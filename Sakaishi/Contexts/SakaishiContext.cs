using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sakaishi.Conversions;
using Sakaishi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sakaishi.Contexts
{
    public class SakaishiContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<LargeCategory> LargeCategories { get; set; }
        public DbSet<SmallCategory> SmallCategories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public SakaishiContext(DbContextOptions<SakaishiContext> options)
            : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<float[]>()
                .HaveConversion<VectorConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(
                t =>
                {
                    t.HasKey(e => e.Id);

                    t.Property(e => e.DateTime)
                    .IsRequired();

                    t.Property(e => e.Title)
                    .IsRequired();

                    t.Property(e => e.Description)
                    .IsRequired();

                    t.Property(e => e.Expense)
                    .IsRequired();

                    t.Property(e => e.Income)
                    .IsRequired();

                    t.Property(e => e.Vector)
                    .IsRequired();
                });

            modelBuilder.Entity<LargeCategory>(
                t =>
                {
                    t.HasKey(e => e.Id);

                    t.Property(e => e.Name)
                    .IsRequired();

                    t.Property(e => e.Vector)
                    .IsRequired();
                });

            modelBuilder.Entity<SmallCategory>(
                t =>
                {
                    t.HasKey(e => e.Id);

                    t.Property(e => e.Name)
                    .IsRequired();

                    t.Property(e => e.Vector)
                    .IsRequired();
                });

            modelBuilder.Entity<PaymentMethod>(
                t =>
                {
                    t.HasKey(e => e.Id);

                    t.Property(e => e.Name)
                    .IsRequired();
                });
        }
    }
}
