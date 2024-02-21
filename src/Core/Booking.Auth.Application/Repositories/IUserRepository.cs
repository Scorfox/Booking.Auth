using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(bool useAsNoTracking, string email, CancellationToken token = default);
    Task<bool> HasAnyByEmailAsync(string email, CancellationToken token = default);
    Task<bool> HasAnyByEmailExceptIdAsync(Guid id, string email, CancellationToken token = default);
    Task<List<User>> GetUsersAsync (int page, int pageSize, CancellationToken token = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
}