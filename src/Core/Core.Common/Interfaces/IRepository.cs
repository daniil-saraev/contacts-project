using Core.Common.Base;

namespace Core.Common.Interfaces;

public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Returns a collection of <typeparamref name="T"/>.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(Func<T, bool>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a <typeparamref name="T"/> entity by id.
    /// </summary>
    Task<T?> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a <typeparamref name="T"/> to the database.
    /// </summary>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a collection of <typeparamref name="T"/> to the database.
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a <typeparamref name="T"/> in the database.
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a collection of <typeparamref name="T"/> in the database.
    /// </summary>
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a <typeparamref name="T"/> from the database.
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a collection of <typeparamref name="T"/> from the database.
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
