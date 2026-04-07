using Yasy.Core.Entities;
using Yasy.Shared.Enums;

namespace Yasy.Core.Interfaces;

public interface IAttendanceRepository
{
    Task<List<AttendanceRecord>> GetByCourseAndRangeAsync(Guid courseId, DateOnly from, DateOnly to, CancellationToken ct = default);
    Task<List<AttendanceRecord>> GetPresentByCustomerAsync(Guid customerId, CancellationToken ct = default);
    Task SetStatusAsync(Guid courseId, Guid customerId, DateOnly date, AttendanceStatus status, CancellationToken ct = default);
    Task RemoveAsync(Guid courseId, Guid customerId, DateOnly date, CancellationToken ct = default);
}
