namespace Yasy.Core.Entities;

public class CourseCancellation
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}
