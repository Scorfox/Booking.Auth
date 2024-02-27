using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface IFilialRepository : IBaseRepository<Filial>
{
    Task<bool> HasAnyWithNameAsync(string inn, CancellationToken cancellationToken = default);
    Task<bool> HasAnyWithNameExceptIdAsync(Guid id, string inn, CancellationToken cancellationToken = default);
    Task<List<Filial>> GetFilialsListAsync(int offset, int limit, CancellationToken cancellationToken = default);

    Task DeleteFilialByIdAsync(Guid id, CancellationToken cancellationToken = default);
}