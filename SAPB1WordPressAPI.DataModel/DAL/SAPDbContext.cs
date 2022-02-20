using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SAPB1WordPressAPI.DataModel.Entities;
using System.Linq;

namespace SAPB1WordPressAPI.DataModel.DAL
{
    public class SapDbContext : DbContext
    {
        public SapDbContext(DbContextOptions<SapDbContext> options) : base(options)
        {

        }

        public virtual DbSet<OCRD> OCRD { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);

                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}
