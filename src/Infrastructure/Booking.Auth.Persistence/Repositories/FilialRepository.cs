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

    public async Task<List<Filial>> GetFilialsListAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await Context.Filials.Skip(offset).Take(limit).ToListAsync(cancellationToken);
    }

    public async Task DeleteFilialByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Context.Filials.Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}