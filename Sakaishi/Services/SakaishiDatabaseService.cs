using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sakaishi.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using SakaishiContext context = await factory.CreateDbContextAsync();
            await context.AddAsync(entity);
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

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(Func<SakaishiContext, DbSet<TEntity>> dbSetSelector, System.Linq.Expressions.Expression<Func<TEntity, TInclude>> includesSelector)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(dbSetSelector);
            ArgumentNullException.ThrowIfNull(includesSelector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            return await dbSetSelector(context).Include(includesSelector)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetEntitiesAsync<TEntity, TInclude>(System.Linq.Expressions.Expression<Func<TEntity, TInclude>> includesSelector)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(includesSelector);

            using SakaishiContext context = await factory.CreateDbContextAsync();

            return await context.Set<TEntity>()
                .Include(includesSelector)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
