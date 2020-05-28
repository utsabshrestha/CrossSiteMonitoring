using Csm.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //cascading off in aspuser with role for role deletion
            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //haha lol take a heck of trial and error to changing name for aspuser table
            builder.Entity<ApplicationUser>(
            b =>
            {
                b.ToTable("user_registration");
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.Email).HasColumnName("email");
                b.Property(p => p.UserName).HasColumnName("username");
            }
            );
        }
    }
}
