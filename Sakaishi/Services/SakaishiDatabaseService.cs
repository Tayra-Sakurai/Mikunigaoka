using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Sakaishi.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Services
{
    public class SakaishiDatabaseService : IDatabaseService<SakaishiContext>
    {
        private readonly IDbContextFactory<SakaishiContext> factory;

        public SakaishiDatabaseService(IDbContextFactory<SakaishiContext> factory)
        {
            this.factory = factory;
            using SakaishiContext context = this.factory.CreateDbContext();
            context.Database.Migrate();
            context.SaveChanges();
        }

        public async Task AddAsync(object entity)
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            EntityEntry entry = context.Add(entity);

            IEnumerable<INavigation> navigations = entry.Metadata.GetNavigations();

            foreach (INavigation navigation in navigations)
            {
                object relatedValue = entry.Navigation(navigation).CurrentValue;

                if (relatedValue is not null)
                {
                    if (!navigation.IsCollection)
                    {
                        EntityEntry relatedEntry = context.Entry(relatedValue);

                        if (relatedEntry.IsKeySet)
                            relatedEntry.State = EntityState.Unchanged;

                        continue;
                    }

                    if (relatedValue is IEnumerable<object> collection)
                        foreach (object item in collection)
                        {
                            EntityEntry entityEntry = context.Entry(item);

                            if (entityEntry.IsKeySet)
                                entityEntry.State = EntityState.Unchanged;

                            continue;
                        }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            EntityEntry<TEntity> entry = context.Add(entity);

            IEnumerable<INavigation> navigations = entry.Metadata.GetNavigations();
            foreach (INavigation navigation in navigations)
            {
                object relatedValue = entry.Navigation(navigation).CurrentValue;
                
                if (relatedValue is not null)
                {
                    if (!navigation.IsCollection)
                    {
                        EntityEntry relatedEntry = context.Entry(relatedValue);

                        if (relatedEntry.IsKeySet)
                            relatedEntry.State = EntityState.Unchanged;
                    }
                    else if (relatedValue is IEnumerable<object> collection)
                        foreach (object item in collection)
                        {
                            EntityEntry itemEntry = context.Entry(item);

                            if (itemEntry.IsKeySet)
                                itemEntry.State = EntityState.Unchanged;
                        }
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(object entity)
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object entity)
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            context.Attach(entity);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            context.Attach(entity);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity>(Func<SakaishiContext, DbSet<TEntity>> factory)
            where TEntity : class
        {
            using SakaishiContext context = await this.factory.CreateDbContextAsync();
            DbSet<TEntity> set = factory(context);
            return await set.ToListAsync();
        }

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity>()
            where TEntity : class
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            DbSet<TEntity> entities = context.Set<TEntity>();
            return await entities.ToListAsync();
        }

        public bool Exists(object entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            using SakaishiContext context = factory.CreateDbContext();
            EntityEntry entry = context.Attach(entity);

            if (entry is null ||
                entry.State == EntityState.Added)
                return false;

            return true;
        }
        
        public bool Exists<TEntity>(TEntity entity)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(entity);
            using SakaishiContext context = factory.CreateDbContext();
            EntityEntry<TEntity> entry = context.Attach(entity);

            if (entry is null ||
                entry.State != EntityState.Unchanged)
                return false;

            return true;
        }

        public async Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity, TFiltered>(TFiltered filter, Func<SakaishiContext, DbSet<TEntity>> factory, Func<TEntity, TFiltered> selector)
            where TEntity : class
            where TFiltered : IEquatable<TFiltered>
        {
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentNullException.ThrowIfNull(factory);
            ArgumentNullException.ThrowIfNull(selector);

            // All entities.
            IList<TEntity> entities = await GetEntitiesAsync(factory);

            List<TEntity> filteredEntities =
                entities.Where(entity => selector(entity).Equals(filter))
                .ToList();

            return filteredEntities;
        }

        public async Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity>(Func<SakaishiContext, DbSet<TEntity>> factory, Func<TEntity, bool> predicate)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(factory);
            ArgumentNullException.ThrowIfNull(predicate);

            IList<TEntity> entities = await GetEntitiesAsync(factory);

            return [.. entities.Where(predicate)];
        }

        public async Task<IList<TEntity>> FilterAndGetEntitiesAsync<TEntity>(Func<TEntity, bool> predicate)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(predicate);

            IList<TEntity> entities = await GetEntitiesAsync<TEntity>();

            return [.. entities.Where(predicate)];
        }

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(Func<SakaishiContext, DbSet<TEntity>> dbSetSelector, Expression<Func<TEntity, TInclude>> includesSelector)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(dbSetSelector);
            ArgumentNullException.ThrowIfNull(includesSelector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            return await dbSetSelector(context).Include(includesSelector)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(Expression<Func<TEntity, TInclude>> includesSelector)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(includesSelector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            return await context.Set<TEntity>()
                .Include(includesSelector)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<object>> GetRelatedEntitiesAsync(object entity, INavigationBase navigation)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(navigation);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            EntityEntry entry = context.Attach(entity);
            return (ICollection<object>)entry.Collection(navigation)
                .CurrentValue;
        }

        public async Task<IEnumerable<TInclude>> GetRelatedEntitiesAsync<TEntity, TInclude>(TEntity entity, Expression<Func<TEntity, IEnumerable<TInclude>>> selector)
            where TEntity : class
            where TInclude : class
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(selector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            EntityEntry<TEntity> entry = context.Attach(entity);

            return await entry.Collection(selector)
                .Query()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<object> GetRelatedEntityAsync(object entity, INavigationBase navigation)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(navigation);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            EntityEntry entry = context.Attach(entity);

            return entry.Reference(navigation).CurrentValue;
        }

        public async Task<TRelated> GetRelatedEntityAsync<TEntity, TRelated>(TEntity entity, Expression<Func<TEntity, TRelated>> selector)
            where TEntity: class
            where TRelated : class
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(selector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            EntityEntry<TEntity> entry = context.Attach(entity);

            return entry.Reference(selector)
                .CurrentValue;
        }

        public System.Collections.IEnumerable GetRelatedEntities(object entity, INavigationBase navigation)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(navigation);

            using SakaishiContext context = factory.CreateDbContext();

            EntityEntry entry = context.Entry(entity);

            return entry.Collection(navigation)
                .CurrentValue;
        }

        public IEnumerable<TInclude> GetRelatedEntities<TEntity, TInclude>(TEntity entity, Expression<Func<TEntity, IEnumerable<TInclude>>> selector)
            where TEntity : class
            where TInclude : class
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(selector);

            using SakaishiContext context = factory.CreateDbContext();

            EntityEntry<TEntity> entityEntry = context.Entry(entity);

            return [.. entityEntry.Collection(selector)
                .Query()
                .AsNoTracking()];
        }

        public object GetRelatedEntity(object entity, INavigationBase navigation)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(navigation);

            using SakaishiContext context = factory.CreateDbContext();

            EntityEntry entry = context.Entry(entity);

            return entry.Reference(navigation)
                .CurrentValue;
        }

        public TRelated GetRelatedEntity<TEntity, TRelated>(TEntity entity, Expression<Func<TEntity, TRelated>> selector)
            where TEntity : class
            where TRelated : class
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(selector);

            using SakaishiContext context = factory.CreateDbContext();

            EntityEntry<TEntity> entityEntry = context.Entry(entity);

            return entityEntry.Reference(selector)
                .CurrentValue;
        }
    }
}
