using Yasy.Core.Entities;

namespace Yasy.Core.Interfaces;

public interface ICourseEnrollmentRepository
{
    Task<List<CourseEnrollment>> GetActiveByCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task<List<CourseEnrollment>> GetAllByCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task<List<CourseEnrollment>> GetActiveByCourseAsync(Guid courseId, CancellationToken ct = default);
    Task<List<CourseEnrollment>> GetAllByCourseAsync(Guid courseId, CancellationToken ct = default);
    Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment, CancellationToken ct = default);
    Task UnenrollAsync(Guid id, CancellationToken ct = default);
}
