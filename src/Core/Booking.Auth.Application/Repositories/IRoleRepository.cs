using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Repositories;

public interface IRoleRepository
{
    public Task<Role> GetByNameAsync(string name, CancellationToken token = default);
}