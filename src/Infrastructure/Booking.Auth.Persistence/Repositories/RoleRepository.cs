using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class RoleRepository(DataContext dataContext) : IRoleRepository
{
    public async Task<Role> GetByNameAsync(string name, CancellationToken token = default)
    {
        return await dataContext.Roles.SingleAsync(r => r.Name == name, token);
    }
}