using Microsoft.EntityFrameworkCore;
using Yasy.Core.Entities;
using Yasy.Core.Interfaces;

namespace Yasy.Infrastructure.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly YasyDbContext _db;

    public CustomerRepository(YasyDbContext db)
    {
        _db = db;
    }

    public async Task<List<Customer>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Customers
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.Customers.FindAsync([id], ct);
    }

    public async Task<List<Customer>> SearchAsync(string searchTerm, CancellationToken ct = default)
    {
        var term = searchTerm.ToLower();
        return await _db.Customers
            .Where(c => c.LastName.ToLower().Contains(term)
                     || c.FirstName.ToLower().Contains(term)
                     || (c.Email != null && c.Email.ToLower().Contains(term))
                     || (c.Phone != null && c.Phone.Contains(term))
                     || (c.City != null && c.City.ToLower().Contains(term)))
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken ct = default)
    {
        customer.CreatedAt = DateTime.UtcNow;
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken ct = default)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var customer = await _db.Customers.FindAsync([id], ct);
        if (customer is not null)
        {
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync(ct);
        }
    }
}
