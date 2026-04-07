namespace Yasy.Core.Entities;

public class CourseCardSharing
{
    public Guid Id { get; set; }
    public Guid CourseCardId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public CourseCard CourseCard { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
}
