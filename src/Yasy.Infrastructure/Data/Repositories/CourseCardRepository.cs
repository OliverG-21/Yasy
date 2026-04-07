using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;
using Yasy.Core.Interfaces;

namespace Yasy.Infrastructure.Data.Repositories;

public class CourseCardRepository : ICourseCardRepository
{
    private readonly YasyDbContext _db;

    public CourseCardRepository(YasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<CourseCard>> GetActiveByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.CourseCards
            .Where(c => c.CustomerId == customerId && c.ExhaustedAt == null)
            .Include(c => c.Usages).ThenInclude(u => u.Course)
            .Include(c => c.Usages).ThenInclude(u => u.UsedByCustomer)
            .Include(c => c.SharedWith).ThenInclude(s => s.Customer)
            .OrderByDescending(c => c.PurchasedAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<CourseCard>> GetAllByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.CourseCards
            .Where(c => c.CustomerId == customerId)
            .Include(c => c.Usages).ThenInclude(u => u.Course)
            .Include(c => c.Usages).ThenInclude(u => u.UsedByCustomer)
            .Include(c => c.SharedWith).ThenInclude(s => s.Customer)
            .OrderByDescending(c => c.PurchasedAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<CourseCard> AddAsync(CourseCard card, CancellationToken ct = default)
    {
        card.CreatedAt = DateTime.UtcNow;
        _db.CourseCards.Add(card);
        await _db.SaveChangesAsync(ct);
        return card;
    }

    public async Task AddUsageAsync(CardUsage usage, CancellationToken ct = default)
    {
        usage.CreatedAt = DateTime.UtcNow;
        _db.CardUsages.Add(usage);

        var card = await _db.CourseCards.FindAsync([usage.CourseCardId], ct);
        if (card is not null)
        {
            card.UsedVisits++;
            if (card.UsedVisits >= card.TotalVisits)
                card.ExhaustedAt = DateOnly.FromDateTime(DateTime.Today);
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task RemoveUsageAsync(Guid customerId, Guid courseId, DateOnly date, CancellationToken ct = default)
    {
        // Find usage: either on customer's own card, or shared usage for this customer
        var usage = await _db.CardUsages
            .Include(u => u.CourseCard)
            .FirstOrDefaultAsync(u =>
                (u.UsedByCustomerId == customerId || (u.UsedByCustomerId == null && u.CourseCard.CustomerId == customerId))
                && u.CourseId == courseId
                && u.UsedAt == date, ct);

        if (usage is not null)
        {
            var card = usage.CourseCard;
            _db.CardUsages.Remove(usage);
            card.UsedVisits = Math.Max(0, card.UsedVisits - 1);
            card.ExhaustedAt = null; // reactivate if it was exhausted
            _db.CourseCards.Update(card);
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task<List<CourseCard>> GetAvailableForCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        // Cards where customer is owner OR explicitly shared with
        return await _db.CourseCards
            .Where(c => c.ExhaustedAt == null &&
                (c.CustomerId == customerId || c.SharedWith.Any(s => s.CustomerId == customerId)))
            .Include(c => c.Usages).ThenInclude(u => u.Course)
            .Include(c => c.Usages).ThenInclude(u => u.UsedByCustomer)
            .Include(c => c.Customer)
            .Include(c => c.SharedWith).ThenInclude(s => s.Customer)
            .OrderBy(c => c.PurchasedAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddSharingAsync(Guid cardId, Guid customerId, CancellationToken ct = default)
    {
        var exists = await _db.CourseCardSharings
            .AnyAsync(s => s.CourseCardId == cardId && s.CustomerId == customerId, ct);
        if (!exists)
        {
            _db.CourseCardSharings.Add(new CourseCardSharing
            {
                CourseCardId = cardId,
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task RemoveSharingAsync(Guid cardId, Guid customerId, CancellationToken ct = default)
    {
        var sharing = await _db.CourseCardSharings
            .FirstOrDefaultAsync(s => s.CourseCardId == cardId && s.CustomerId == customerId, ct);
        if (sharing is not null)
        {
            _db.CourseCardSharings.Remove(sharing);
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task<List<CourseCard>> GetLowBalanceCardsAsync(int maxRemaining = 2, CancellationToken ct = default)
    {
        return await _db.CourseCards
            .Where(c => c.ExhaustedAt == null && (c.TotalVisits - c.UsedVisits) <= maxRemaining && (c.TotalVisits - c.UsedVisits) > 0)
            .Include(c => c.Customer)
            .OrderBy(c => c.TotalVisits - c.UsedVisits)
            .ThenBy(c => c.Customer.LastName)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
