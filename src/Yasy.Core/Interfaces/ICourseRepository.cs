using Yasy.Core.Entities;
using Yasy.Shared.Enums;

namespace Yasy.Core.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync(CancellationToken ct = default);
    Task<List<Course>> GetByTypeAsync(CourseType type, CancellationToken ct = default);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Course>> SearchAsync(string searchTerm, CancellationToken ct = default);
    Task<Course> AddAsync(Course course, CancellationToken ct = default);
    Task UpdateAsync(Course course, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<HashSet<(Guid CourseId, DateOnly Date)>> GetCancellationsAsync(DateOnly from, DateOnly to, CancellationToken ct = default);
    Task CancelOccurrenceAsync(Guid courseId, DateOnly date, CancellationToken ct = default);
    Task UncancelOccurrenceAsync(Guid courseId, DateOnly date, CancellationToken ct = default);
}
