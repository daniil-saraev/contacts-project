﻿using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityAPI.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            //builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
            }
        }
    }
}
