using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FakeCSV.DAL.Context
{
    public class FakeCsvDbContext: IdentityDbContext
    {

        public FakeCsvDbContext(DbContextOptions<FakeCsvDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);
    }
}
