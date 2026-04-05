using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        /// Asynchronously loads the entities with the relations included.
        /// </summary>
        /// <typeparam name="TEntity">The target entity type.</typeparam>
        /// <typeparam name="TInclude">The property value type of the navigation properties.</typeparam>
        /// <param name="dbSetSelector">The selector function of the <see cref="DbSet{TEntity}"/>.</param>
        /// <param name="includesSelector">Inclusion selector function.</param>
        /// <returns>The task to manage the asynchronous action.</returns>
        /// <exception cref="ArgumentNullException">One argument is null.</exception>
        Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(Func<TContext, DbSet<TEntity>> dbSetSelector, System.Linq.Expressions.Expression<Func<TEntity, TInclude>> includesSelector)
            where TEntity : class;

        /// <inheritdoc cref="GetEntitiesAsync{TEntity, TInclude}(Func{TContext, DbSet{TEntity}}, System.Linq.Expressions.Expression{Func{TEntity, TInclude}})"/>
        Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(System.Linq.Expressions.Expression<Func<TEntity, TInclude>> includesSelector)
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

        /// <summary>
        /// Asynchronously retrieves a list of entities that match the specified filter criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve. Must be a reference type.</typeparam>
        /// <typeparam name="TFiltered">The type of the filter used to select entities.</typeparam>
        /// <param name="filter">The filter criteria used to select entities. Determines which entities are included in the result.</param>
        /// <param name="entityFactory">A function that provides access to the DbSet for the specified entity type from the database context.</param>
        /// <param name="selector">A function that projects an entity to its corresponding filter type. Used to compare entities against the
        /// provided filter.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy
        /// the filter criteria. If no entities match, the list is empty.</returns>
        /// <exception cref="ArgumentNullException">One or more arguments are null.</exception>
        /// <exception cref="ArgumentException">Parameters contain one or more invalid ones.</exception>
        Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity, TFiltered>(TFiltered filter, Func<TContext, DbSet<TEntity>> entityFactory, Func<TEntity, TFiltered> selector)
            where TEntity : class
            where TFiltered : IEquatable<TFiltered>;

        /// <summary>
        /// Asynchronously retrieves a list of entities of type <typeparamref name="TEntity"/> that satisfy the specified predicate from the
        /// provided DbSet factory.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to retrieve. Must be a reference type.</typeparam>
        /// <param name="factory">A function that, given a database context, returns the DbSet representing the collection of entities to
        /// query.</param>
        /// <param name="predicate">A function that defines the conditions each entity must satisfy to be included in the result.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities of type
        /// <typeparamref name="TEntity"/> that match the <paramref name="predicate"/>. If no entities match, the list is empty.</returns>
        /// <exception cref="ArgumentNullException">One or more arguments are null.</exception>
        /// <exception cref="ArgumentException">One or more arguments are invalid.</exception>
        Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity>(Func<TContext, DbSet<TEntity>> factory, Func<TEntity, bool> predicate)
            where TEntity : class;

        /// <summary>
        /// Asynchronously retrieves the entities with matched type of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <inheritdoc cref="FilterAndGetEntitiesAsync{TEntity}(Func{TContext, DbSet{TEntity}}, Func{TEntity, bool})"/>
        Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity>(Func<TEntity, bool> predicate)
            where TEntity : class;

        /// <summary>
        /// Asynchronously retrieves a collection of related entities for the specified entity using the provided
        /// selector expression.
        /// </summary>
        /// <param name="entity">The source entity for which related entities are to be retrieved. Cannot be null.</param>
        /// <param name="navigation">A navigation to the returning entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of related
        /// entities. The collection is empty if no related entities are found.</returns>
        /// <exception cref="ArgumentNullException">One or more arguments are null.</exception>
        /// <exception cref="InvalidOperationException">The extraction of the collection has failed.</exception>
        Task<ICollection<object>> GetRelatedEntitiesAsync(object entity, INavigation navigation);

        /// <summary>
        /// Asynchronously gets the related entities from <paramref name="entity"/> with specified by <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TInclude">The type of the related entity to be loaded.</typeparam>
        /// <param name="entity">The entity to load the including entities.</param>
        /// <param name="selector">The specifier of the inclusion.</param>
        /// <returns>A task which represents the asynchronous operation. The task result contains the collection of the related entities.</returns>
        /// <exception cref="ArgumentNullException">One or more arguments are null.</exception>
        /// <exception cref="InvalidOperationException">The inclusion finder failed to get the related entities.</exception>
        Task<IEnumerable<TInclude>> GetRelatedEntitiesAsync<TEntity, TInclude>(TEntity entity, System.Linq.Expressions.Expression<Func<TEntity, IEnumerable<TInclude>>> selector)
            where TEntity : class
            where TInclude : class;
    }
}
