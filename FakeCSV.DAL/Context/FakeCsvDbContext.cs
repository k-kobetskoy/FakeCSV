using System;
using System.Collections.Generic;
using System.Text;
using FakeCSV.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace FakeCSV.DAL.Context
{
    public class FakeCsvDbContext: IdentityDbContext
    {

        public DbSet<Schema> Schemas { get; set; }
        public DbSet<Column> Columns { get; set; }

        public FakeCsvDbContext(DbContextOptions<FakeCsvDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Schema>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Column>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Schema>().HasMany(e => e.Columns).WithOne(e => e.Schema);

            modelBuilder
                .Entity<Schema>()
                .Property(p => p.Quotation).HasMaxLength(15)
                .HasConversion(
                    v => v.ToString(),
                    v => (QuotationMark) Enum.Parse(typeof(QuotationMark), v));
            modelBuilder
                .Entity<Schema>()
                .Property(p => p.Separator).HasMaxLength(15)
                .HasConversion(
                    v => v.ToString(),
                    v => (ColumnSeparator) Enum.Parse(typeof(ColumnSeparator), v));
            
            modelBuilder
                .Entity<Column>()
                .Property(p => p.Type).HasMaxLength(15)
                .HasConversion(
                    v => v.ToString(),
                    v => (ColumnType) Enum.Parse(typeof(ColumnType), v));

        }
    }
}
