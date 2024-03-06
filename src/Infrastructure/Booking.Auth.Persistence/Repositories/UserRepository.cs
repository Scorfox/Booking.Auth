using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class UserRepository(DataContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmailAsync(bool useAsNoTracking, string email, CancellationToken cancellationToken)
    {
        if (useAsNoTracking)
            return await Context.Users
                .Include(e => e.Role)
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        
        return await Context.Users
            .Include(e => e.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> HasAnyByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> HasAnyByEmailExceptIdAsync(Guid id, string email, CancellationToken cancellationToken)
    {
        return await Context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Email.ToLower() == email, cancellationToken);
    }

    public async Task<Tuple<List<User>, int>> GetUsersAsync(int offset, int limit, CancellationToken token = default)
    {
        return new Tuple<List<User>, int>(await Context.Users.Skip(offset).Take(limit).ToListAsync(token), await Context.Users.CountAsync(token));
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        await Context.Users.Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}