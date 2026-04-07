namespace Yasy.Core.Entities;

public class CardUsage
{
    public Guid Id { get; set; }
    public Guid CourseCardId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? UsedByCustomerId { get; set; }
    public DateOnly UsedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public CourseCard CourseCard { get; set; } = default!;
    public Course? Course { get; set; }
    public Customer? UsedByCustomer { get; set; }
}
