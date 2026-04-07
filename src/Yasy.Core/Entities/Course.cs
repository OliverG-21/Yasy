using Yasy.Shared.Enums;

namespace Yasy.Core.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CourseType Type { get; set; }

    // Workshop fields
    public DateOnly? Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public decimal? Price { get; set; }

    // Regular course fields
    public DayOfWeek? DayOfWeek { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal? DropInPrice { get; set; }
    public decimal? FiveCardPrice { get; set; }
    public decimal? TenCardPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<CourseEnrollment> Enrollments { get; set; } = [];
}
