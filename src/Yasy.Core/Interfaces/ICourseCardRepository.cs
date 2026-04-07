using Yasy.Core.Entities;

namespace Yasy.Core.Interfaces;

public interface ICourseCardRepository
{
    Task<List<CourseCard>> GetActiveByCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task<List<CourseCard>> GetAllByCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task<CourseCard> AddAsync(CourseCard card, CancellationToken ct = default);
    Task AddUsageAsync(CardUsage usage, CancellationToken ct = default);
    Task RemoveUsageAsync(Guid customerId, Guid courseId, DateOnly date, CancellationToken ct = default);
    Task<List<CourseCard>> GetAvailableForCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task AddSharingAsync(Guid cardId, Guid customerId, CancellationToken ct = default);
    Task RemoveSharingAsync(Guid cardId, Guid customerId, CancellationToken ct = default);
    Task<List<CourseCard>> GetLowBalanceCardsAsync(int maxRemaining = 2, CancellationToken ct = default);
}
