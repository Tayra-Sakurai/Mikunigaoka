using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Services
{
    public interface IDatabaseService<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Asynchronously adds the specified entity to the underlying data store.
        /// </summary>
        /// <param name="entity">The entity to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="entity"/>'s type does not exists in <typeparamref name="TContext"/>.</exception>
        Task AddAsync(object entity);

        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <inheritdoc cref="AddAsync(object)"/>
        Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Asynchronously updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity object to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        /// <inheritdoc cref="AddAsync(object)"/>
        Task UpdateAsync(object entity);

        /// <typeparam name="TEntity">The updating entity type.</typeparam>
        /// <inheritdoc cref="UpdateAsync(object)"/>
        Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// Asynchronously deletes the specified entity from the data store.
        /// </summary>
        /// <param name="entity">The entity object to be deleted. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <inheritdoc cref="AddAsync(object)"/>
        Task DeleteAsync(object entity);

        /// <typeparam name="TEntity">The type of the entity to be removed.</typeparam>
        /// <inheritdoc cref="DeleteAsync(object)"/>
        Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
