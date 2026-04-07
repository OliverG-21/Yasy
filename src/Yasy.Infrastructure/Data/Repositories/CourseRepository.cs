using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;
using Yasy.Core.Interfaces;
using Yasy.Shared.Enums;

namespace Yasy.Infrastructure.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly YasyDbContext _db;

    public CourseRepository(YasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<Course>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Courses
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<Course>> GetByTypeAsync(CourseType type, CancellationToken ct = default)
    {
        return await _db.Courses
            .Where(c => c.Type == type)
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Courses.FindAsync([id], ct);
    }

    public async Task<List<Course>> SearchAsync(string searchTerm, CancellationToken ct = default)
    {
        var term = searchTerm.ToLower();
        return await _db.Courses
            .Where(c => c.Name.ToLower().Contains(term))
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Course> AddAsync(Course course, CancellationToken ct = default)
    {
        course.CreatedAt = DateTime.UtcNow;
        _db.Courses.Add(course);
        await _db.SaveChangesAsync(ct);
        return course;
    }

    public async Task UpdateAsync(Course course, CancellationToken ct = default)
    {
        course.UpdatedAt = DateTime.UtcNow;
        _db.Courses.Update(course);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var course = await _db.Courses.FindAsync([id], ct);
        if (course is not null)
        {
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task<HashSet<(Guid CourseId, DateOnly Date)>> GetCancellationsAsync(DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var cancellations = await _db.CourseCancellations
            .Where(c => c.Date >= from && c.Date <= to)
            .AsNoTracking()
            .ToListAsync(ct);

        return cancellations.Select(c => (c.CourseId, c.Date)).ToHashSet();
    }

    public async Task CancelOccurrenceAsync(Guid courseId, DateOnly date, CancellationToken ct = default)
    {
        var exists = await _db.CourseCancellations
            .AnyAsync(c => c.CourseId == courseId && c.Date == date, ct);

        if (!exists)
        {
            _db.CourseCancellations.Add(new CourseCancellation
            {
                CourseId = courseId,
                Date = date,
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task UncancelOccurrenceAsync(Guid courseId, DateOnly date, CancellationToken ct = default)
    {
        var cancellation = await _db.CourseCancellations
            .FirstOrDefaultAsync(c => c.CourseId == courseId && c.Date == date, ct);

        if (cancellation is not null)
        {
            _db.CourseCancellations.Remove(cancellation);
            await _db.SaveChangesAsync(ct);
        }
    }
}
