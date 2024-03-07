using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<bool> HasAnyWithInnAsync(string inn, CancellationToken cancellationToken = default);
    Task<bool> HasAnyWithInnExceptIdAsync(Guid id, string inn, CancellationToken cancellationToken = default);
    Task<List<Company>> GetAllCompaniesAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task<int> GetCompaniesTotalCountAsync(CancellationToken cancellationToken = default);
    Task DeleteCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default);
}