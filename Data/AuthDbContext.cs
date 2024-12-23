using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.Data
{
    public class AuthDbContext : IdentityDbContext
    {
     public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
     {
     }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "f8e3b1e3-6b6d-4b3b-8b1b-3e1f0b5b6e1b";
            var writerRoleId = "f8e3b1e3-6b6d-4b3b-8b1b-3e1f0b5b6e1c";
            var roles = new List<IdentityRole> {
                new IdentityRole {
                    Id = readerRoleId, 
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER".ToUpper()
                },
                new IdentityRole {
                    Id = writerRoleId, 
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }

}