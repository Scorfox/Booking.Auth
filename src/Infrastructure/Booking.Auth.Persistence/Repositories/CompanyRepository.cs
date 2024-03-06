﻿using Booking.Auth.Application.Repositories;
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

    public async Task<Tuple<List<Company>, int>> GetAllCompaniesAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return new Tuple<List<Company>, int>(await Context.Companies.Skip(offset).Take(limit).ToListAsync(cancellationToken), await Context.Companies.CountAsync(cancellationToken));
    }

    public async Task DeleteCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        bool any = await Context.Companies.AnyAsync(elm => elm.Id == id, cancellationToken);

        if (any)
            await Context.Companies.Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}