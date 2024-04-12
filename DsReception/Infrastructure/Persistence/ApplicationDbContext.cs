using DsReceptionClassLibrary.Domain.Entities.Clients;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DsReceptionAPI.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Client>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Image>().HasKey(e => new { e.ClientId, e.ImageId });
            builder.Entity<Login>().HasKey(e => new { e.ClientId, e.LoginId });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Company> Companys { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
