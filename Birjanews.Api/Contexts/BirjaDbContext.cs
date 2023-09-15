using System;
using Birjanews.Api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Birjanews.Api.Contexts
{
   

    public class BirjaDbContext:IdentityDbContext
	{
		public BirjaDbContext(DbContextOptions options):base(options)
		{

		}
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<Organizer> Organizers { get; set; }
		public DbSet<Advertisement> Advertisements { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<Message> Messages { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow.AddHours(4);
                        break;
                    case EntityState.Modified:
                        entry.Entity.CreatedDate = DateTime.UtcNow.AddHours(4);
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}

