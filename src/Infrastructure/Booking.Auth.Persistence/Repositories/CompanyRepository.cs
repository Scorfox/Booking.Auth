using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Repositories;

public class CompanyRepository(DataContext context) : BaseRepository<Company>(context), ICompanyRepository
{
    public async Task<bool> HasAnyWithInnAsync(string inn, CancellationToken cancellationToken = default)
    {
        return await Context.Companies
            .AsNoTracking()
            .AnyAsync(x => x.Inn == inn, cancellationToken);
    }

    public async Task<bool> HasAnyWithInnExceptIdAsync(Guid id, string inn, CancellationToken cancellationToken = default)
    {
        return await Context.Companies
            .AsNoTracking()
            .AnyAsync(x => x.Id != id && x.Inn == inn, cancellationToken);
    }

    public async Task<List<Company>> GetAllCompaniesAsync(int offset, int limit, CancellationToken cancellationToken = default) 
        => await base.GetPaginatedListAsync(offset, limit, cancellationToken);

    public async Task<int> GetCompaniesTotalCountAsync(CancellationToken cancellationToken = default)
        => await base.GetTotalCount(cancellationToken);
    
    public async Task DeleteCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool any = await Context.Companies.AnyAsync(elm => elm.Id == id, cancellationToken);

        if (any)
            await Context.Companies.Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}