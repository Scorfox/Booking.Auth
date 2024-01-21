using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(bool useAsNoTracking, string email, CancellationToken token);
    Task<bool> HasAnyByEmailAsync(string email, CancellationToken token);
}