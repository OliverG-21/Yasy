using Yasy.Core.Entities;

namespace Yasy.Core.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<Customer>> SearchAsync(string searchTerm, CancellationToken ct = default);
    Task<List<Customer>> GetUpcomingBirthdaysAsync(int days = 7, CancellationToken ct = default);
    Task<Customer> AddAsync(Customer customer, CancellationToken ct = default);
    Task UpdateAsync(Customer customer, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
