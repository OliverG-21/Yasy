using Yasy.Shared.Enums;

namespace Yasy.Core.Entities;

public class CourseCard
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public CardType Type { get; set; }
    public int TotalVisits { get; set; }
    public int UsedVisits { get; set; }
    public decimal PricePaid { get; set; }
    public DateOnly PurchasedAt { get; set; }
    public DateOnly? ExhaustedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public int RemainingVisits => TotalVisits - UsedVisits;

    public Customer Customer { get; set; } = default!;
    public List<CardUsage> Usages { get; set; } = [];
    public List<CourseCardSharing> SharedWith { get; set; } = [];
}
