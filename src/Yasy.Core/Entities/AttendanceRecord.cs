using Yasy.Shared.Enums;

namespace Yasy.Core.Entities;

public class AttendanceRecord
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid CustomerId { get; set; }
    public DateOnly Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Course Course { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
}
