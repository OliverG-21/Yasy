using Heron.MudCalendar;

namespace Yasy.Server.Models;

public class CourseCalendarItem : CalendarItem
{
    public Guid? SourceId { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }
}
