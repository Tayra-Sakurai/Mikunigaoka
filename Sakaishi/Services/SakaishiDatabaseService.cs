using Microsoft.EntityFrameworkCore;
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
    }
}
