using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Services
{
    /// <summary>
    /// Defines a contract for performing asynchronous CRUD operations on entities within a specified Entity Framework
    /// database context.
    /// </summary>
    /// <remarks>Implementations of this interface provide generic methods for adding, updating, deleting, and
    /// retrieving entities in a type-safe manner using the specified DbContext. All operations are asynchronous and
    /// designed to work with Entity Framework's change tracking and data access patterns.</remarks>
    /// <typeparam name="TContext">The type of the Entity Framework database context. Must derive from DbContext.</typeparam>
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

        /// <summary>
        /// <para>Gets the list of <typeparamref name="TEntity"/> which is contained by <typeparamref name="TContext"/>.</para>
        /// <para>This first applies <paramref name="factory"/> to the <typeparamref name="TContext"/> instance held by the class implemention. Next, this gets the list from the <see cref="DbSet{TEntity}"/>.</para>
        /// </summary>
        /// <remarks>
        /// Since this function uses a <see cref="DbSet{TEntity}"/> instance, <typeparamref name="TEntity"/> must be a class.
        /// </remarks>
        /// <typeparam name="TEntity">The entity type. Must be a reference type.</typeparam>
        /// <param name="factory">A function to get the <see cref="DbSet{TEntity}"/> instance.</param>
        /// <returns>A task to manage the asynchronous operation to return an instance of an implemention of <see cref="IList{TEntity}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        Task<IList<TEntity>> GetEntitiesAsync<TEntity>(Func<TContext, DbSet<TEntity>> factory)
            where TEntity : class;

        /// <summary>
        /// Asynchronously retrieves a collection of entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to retrieve. Must be a reference type.</typeparam>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities of type
        /// TEntity.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="TEntity"/> does not exists in the database models.</exception>
        Task<IList<TEntity>> GetEntitiesAsync<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Checks if <paramref name="entity"/> exists or not.
        /// </summary>
        /// <param name="entity">The entity to be checked its existence.</param>
        /// <returns>true if <paramref name="entity"/> exists in the database referred by the instance of <typeparamref name="TContext"/> in the implemention; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The operation is invalid.</exception>
        bool Exists(object entity);

        /// <typeparam name="TEntity">The refrerred entity type.</typeparam>
        /// <inheritdoc cref="Exists(object)"/>
        bool Exists<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
