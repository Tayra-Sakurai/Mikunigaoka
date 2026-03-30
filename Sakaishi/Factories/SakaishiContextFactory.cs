using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sakaishi.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Factories
{
    public class SakaishiContextFactory : IDesignTimeDbContextFactory<SakaishiContext>
    {
        public SakaishiContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SakaishiContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=Sakaishi.db");

            return new(optionsBuilder.Options);
        }
    }
}
