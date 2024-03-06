using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<bool> HasAnyWithInnAsync(string inn, CancellationToken cancellationToken = default);
    Task<bool> HasAnyWithInnExceptIdAsync(Guid id, string inn, CancellationToken cancellationToken = default);
    Task<Tuple<List<Company>, int>> GetAllCompaniesAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task DeleteCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default);
}