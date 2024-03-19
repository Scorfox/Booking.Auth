using System.Linq.Expressions;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Common;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;

    protected BaseRepository(DataContext context)
    {
        Context = context;
    }
    
    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> HasAnyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetPaginatedListAsync(int offset, int count, Expression<Func<T, bool>>? expression, CancellationToken token)
    {
        var query = expression == null ? Context.Set<T>() : Context.Set<T>().Where(expression);
        return await query.Skip(offset).Take(count).ToListAsync(token);
    }

    public async Task<int> GetTotalCount(CancellationToken token)
    {
        return await Context.Set<T>().CountAsync(token);
    }
}