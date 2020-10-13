using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Souq.Tables;
using System;


namespace Souq.DataAccessLayer
{
    public class SouqDB : IdentityDbContext
    {
        public SouqDB(DbContextOptions<SouqDB> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

       public DbSet<Product> products { get; set; }
       public  DbSet<User> users { get; set; }


    }
}
