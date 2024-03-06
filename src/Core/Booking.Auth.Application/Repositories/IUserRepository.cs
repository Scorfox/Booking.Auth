using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(bool useAsNoTracking, string email, CancellationToken token = default);
    Task<bool> HasAnyByEmailAsync(string email, CancellationToken token = default);
    Task<bool> HasAnyByEmailExceptIdAsync(Guid id, string email, CancellationToken token = default);
    Task<Tuple<List<User>, int>> GetUsersAsync (int offset, int limit, CancellationToken token = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
}