using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class 
    {    
        /// <summary>
        /// Returns a collection of <typeparamref name="T"/>.
        /// </summary>
        Task<IEnumerable<T>?> GetAllAsync();

        /// <summary>
        /// Returns a <typeparamref name="T"/> entity by id.
        /// </summary>
        Task<T?> GetAsync(int id);

        /// <summary>
        /// Adds a <typeparamref name="T"/> to the database.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds a collection of <typeparamref name="T"/> to the database.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates a <typeparamref name="T"/> in the database.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates a collection of <typeparamref name="T"/> in the database.
        /// </summary>
        Task UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Removes a <typeparamref name="T"/> from the database.
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Removes a collection of <typeparamref name="T"/> from the database.
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}
