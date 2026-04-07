using Yasy.Core.Interfaces;
using Yasy.Server.Models;
using Yasy.Shared.Enums;

namespace Yasy.Server.Services;

public class CalendarService
{
    private readonly ICourseRepository _courseRepo;
    private readonly IAttendanceRepository _attendanceRepo;
    private readonly ICourseCardRepository _cardRepo;

    public CalendarService(ICourseRepository courseRepo, IAttendanceRepository attendanceRepo, ICourseCardRepository cardRepo)
    {
        _courseRepo = courseRepo;
        _attendanceRepo = attendanceRepo;
        _cardRepo = cardRepo;
    }

    public async Task<List<CourseCalendarItem>> GetEventsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var courses = await _courseRepo.GetAllAsync(ct);
        var cancellations = await _courseRepo.GetCancellationsAsync(from, to, ct);
        var items = new List<CourseCalendarItem>();

        foreach (var course in courses)
        {
            if (course.Type == CourseType.Workshop)
            {
                if (course.Date.HasValue && course.Date.Value >= from && course.Date.Value <= to)
                {
                    var isCancelled = cancellations.Contains((course.Id, course.Date.Value));
                    var start = course.Date.Value.ToDateTime(course.StartTime ?? new TimeOnly(0, 0));
                    var end = course.EndTime.HasValue
                        ? course.Date.Value.ToDateTime(course.EndTime.Value)
                        : start.AddHours(1);

                    items.Add(new CourseCalendarItem
                    {
                        Text = isCancelled ? $"❌ {course.Name}" : course.Name,
                        Start = start,
                        End = end,
                        Category = "Workshop",
                        SourceId = course.Id,
                        IsCancelled = isCancelled
                    });
                }
            }
            else if (course.DayOfWeek.HasValue)
            {
                var current = from;
                while (current.DayOfWeek != course.DayOfWeek.Value && current <= to)
                    current = current.AddDays(1);

                while (current <= to)
                {
                    var isCancelled = cancellations.Contains((course.Id, current));
                    var startTime = course.StartTime ?? new TimeOnly(0, 0);
                    var start = current.ToDateTime(startTime);
                    var end = course.DurationMinutes.HasValue
                        ? start.AddMinutes(course.DurationMinutes.Value)
                        : start.AddHours(1);

                    items.Add(new CourseCalendarItem
                    {
                        Text = isCancelled ? $"❌ {course.Name}" : course.Name,
                        Start = start,
                        End = end,
                        Category = "Kurs",
                        SourceId = course.Id,
                        IsCancelled = isCancelled
                    });

                    current = current.AddDays(7);
                }
            }
        }

        return items.OrderBy(e => e.Start).ToList();
    }

    public async Task ToggleCancellationAsync(Guid courseId, DateOnly date, bool isCancelled, CancellationToken ct = default)
    {
        if (isCancelled)
        {
            // Reactivate: just remove the cancellation
            await _courseRepo.UncancelOccurrenceAsync(courseId, date, ct);
        }
        else
        {
            // Cancel: remove all attendance records and reverse card usages for this date
            var records = await _attendanceRepo.GetByCourseAndRangeAsync(courseId, date, date, ct);
            foreach (var record in records.Where(r => r.Status == AttendanceStatus.Present))
            {
                await _cardRepo.RemoveUsageAsync(record.CustomerId, courseId, date, ct);
            }
            // Remove all attendance records for this date
            foreach (var record in records)
            {
                await _attendanceRepo.RemoveAsync(courseId, record.CustomerId, date, ct);
            }

            await _courseRepo.CancelOccurrenceAsync(courseId, date, ct);
        }
    }
}
