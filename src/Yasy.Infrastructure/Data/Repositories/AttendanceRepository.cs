using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;
using Yasy.Core.Interfaces;
using Yasy.Shared.Enums;

namespace Yasy.Infrastructure.Data.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly YasyDbContext _db;

    public AttendanceRepository(YasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<AttendanceRecord>> GetByCourseAndRangeAsync(Guid courseId, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        return await _db.AttendanceRecords
            .Where(a => a.CourseId == courseId && a.Date >= from && a.Date <= to)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<AttendanceRecord>> GetPresentByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.AttendanceRecords
            .Where(a => a.CustomerId == customerId && a.Status == AttendanceStatus.Present)
            .OrderBy(a => a.Date)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task SetStatusAsync(Guid courseId, Guid customerId, DateOnly date, AttendanceStatus status, CancellationToken ct = default)
    {
        var record = await _db.AttendanceRecords
            .FirstOrDefaultAsync(a => a.CourseId == courseId && a.CustomerId == customerId && a.Date == date, ct);

        if (record is not null)
        {
            record.Status = status;
            _db.AttendanceRecords.Update(record);
        }
        else
        {
            _db.AttendanceRecords.Add(new AttendanceRecord
            {
                CourseId = courseId,
                CustomerId = customerId,
                Date = date,
                Status = status,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Guid courseId, Guid customerId, DateOnly date, CancellationToken ct = default)
    {
        var record = await _db.AttendanceRecords
            .FirstOrDefaultAsync(a => a.CourseId == courseId && a.CustomerId == customerId && a.Date == date, ct);

        if (record is not null)
        {
            _db.AttendanceRecords.Remove(record);
            await _db.SaveChangesAsync(ct);
        }
    }
}
