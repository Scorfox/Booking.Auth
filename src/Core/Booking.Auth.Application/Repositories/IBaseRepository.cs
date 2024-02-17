﻿namespace Booking.Auth.Application.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task Delete(T entity);
    Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasAnyByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
}