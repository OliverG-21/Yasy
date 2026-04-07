namespace Yasy.Core.Entities;

public class CourseEnrollment
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CourseId { get; set; }
    public DateOnly EnrolledAt { get; set; }
    public DateOnly? UnenrolledAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
