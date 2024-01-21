namespace Booking.Auth.Application.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
}