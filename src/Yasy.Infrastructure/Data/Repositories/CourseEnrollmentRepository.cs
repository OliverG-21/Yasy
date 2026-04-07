using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;
using Yasy.Core.Interfaces;

namespace Yasy.Infrastructure.Data.Repositories;

public class CourseEnrollmentRepository : ICourseEnrollmentRepository
{
    private readonly YasyDbContext _db;

    public CourseEnrollmentRepository(YasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<CourseEnrollment>> GetActiveByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.CourseEnrollments
            .Where(e => e.CustomerId == customerId && e.UnenrolledAt == null)
            .Include(e => e.Course)
            .OrderBy(e => e.Course.Name)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<CourseEnrollment>> GetAllByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.CourseEnrollments
            .Where(e => e.CustomerId == customerId)
            .Include(e => e.Course)
            .OrderByDescending(e => e.EnrolledAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<CourseEnrollment>> GetActiveByCourseAsync(Guid courseId, CancellationToken ct = default)
    {
        return await _db.CourseEnrollments
            .Where(e => e.CourseId == courseId && e.UnenrolledAt == null)
            .Include(e => e.Customer)
            .OrderBy(e => e.Customer.LastName)
            .ThenBy(e => e.Customer.FirstName)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<List<CourseEnrollment>> GetAllByCourseAsync(Guid courseId, CancellationToken ct = default)
    {
        return await _db.CourseEnrollments
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Customer)
            .OrderByDescending(e => e.EnrolledAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment, CancellationToken ct = default)
    {
        enrollment.CreatedAt = DateTime.UtcNow;
        _db.CourseEnrollments.Add(enrollment);
        await _db.SaveChangesAsync(ct);
        return enrollment;
    }

    public async Task UnenrollAsync(Guid id, CancellationToken ct = default)
    {
        var enrollment = await _db.CourseEnrollments.FindAsync([id], ct);
        if (enrollment is not null)
        {
            enrollment.UnenrolledAt = DateOnly.FromDateTime(DateTime.Today);
            _db.CourseEnrollments.Update(enrollment);
            await _db.SaveChangesAsync(ct);
        }
    }
}
