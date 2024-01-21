using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class UserRepository(DataContext context) : BaseRepository<User>(context), IUserRepository
{
    public Task<User?> FindByEmailAsync(bool useAsNoTracking, string email, CancellationToken cancellationToken)
    {
        if (useAsNoTracking)
            return Context.Users
                .Include(e => e.Role)
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        
        return 
            Context.Users
            .Include(e => e.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public Task<bool> HasAnyByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return Context.Users
            .AnyAsync(x => x.Email == email, cancellationToken);
    }
}