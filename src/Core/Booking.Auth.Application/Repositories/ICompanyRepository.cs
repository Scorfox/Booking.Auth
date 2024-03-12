using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<bool> HasAnyWithInnAsync(string inn, CancellationToken cancellationToken = default);
    Task<bool> HasAnyWithInnExceptIdAsync(Guid id, string inn, CancellationToken cancellationToken = default);
}