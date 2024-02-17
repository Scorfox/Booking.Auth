using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class FilialRepository(DataContext context) : BaseRepository<Filial>(context), IFilialRepository
{
    public async Task<bool> HasAnyWithNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Filials
            .AsNoTracking()
            .AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<bool> HasAnyWithNameExceptIdAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        return await Context.Filials
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Name == name, cancellationToken);
    }
}