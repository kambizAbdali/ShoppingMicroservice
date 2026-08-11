using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    /// <summary>
    /// DbContext مربوط به میکروسرویس Ordering
    /// </summary>
    public class OrderContext(DbContextOptions<OrderContext> options)
        : DbContext(options), IUnitOfWork
    {
        public DbSet<Order> Orders => Set<Order>();

        public override int SaveChanges()
        {
            ApplyAuditInformation();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();

            return base.SaveChangesAsync(
                acceptAllChangesOnSuccess,
                cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(order => order.Id);

                entity.Property(order => order.TotalPrice)
                    .HasPrecision(18, 2);

                entity.Property(order => order.PaymentMethod)
                    .HasConversion<int>();

                entity.Property(order => order.Version)
                    .IsConcurrencyToken();

                entity.Property(order => order.UserName)
                    .HasMaxLength(100);

                entity.Property(order => order.EmailAddress)
                    .HasMaxLength(200);

                entity.Property(order => order.FirstName)
                    .HasMaxLength(100);

                entity.Property(order => order.LastName)
                    .HasMaxLength(100);

                entity.Property(order => order.State)
                    .HasMaxLength(100);

                entity.Property(order => order.AddressLine)
                    .HasMaxLength(500);
            });
        }

        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(entry =>
                    entry.State is EntityState.Added
                    or EntityState.Modified
                    or EntityState.Deleted);

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedDate = null;
                        entry.Entity.Version = 1;
                        entry.Entity.IsActive = true;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.Version++;

                        entry.Property(entity => entity.Id)
                            .IsModified = false;

                        entry.Property(entity => entity.CreatedDate)
                            .IsModified = false;

                        entry.Property(entity => entity.CreatedBy)
                            .IsModified = false;
                        break;

                    case EntityState.Deleted:
                        // Convert physical delete to soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.IsActive = false;
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entry.Entity.Version++;

                        entry.Property(entity => entity.Id)
                            .IsModified = false;

                        entry.Property(entity => entity.CreatedDate)
                            .IsModified = false;

                        entry.Property(entity => entity.CreatedBy)
                            .IsModified = false;
                        break;
                }
            }
        }
    }
}
