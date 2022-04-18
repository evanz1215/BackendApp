using Application.Common.Dependencies.Services;
using Domain.Common;
using Infrastructure.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole,
       IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUser, IDateTime dateTime) : base(options)
        {
            _currentUser = currentUser;
            _dateTime = dateTime;

            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //ConfigureValueObjects(builder);
            //ConfigureDecimalPrecision(builder);
            ConfigureSoftDeleteFilter(builder);

            string schema = "dbo";
            builder.HasDefaultSchema(schema);

            //builder.ApplyConfiguration(new ProductTypeConfiguration());

            base.OnModelCreating(builder);
            //builder.HasSequence<int>("ProductSequence", schema);
            //builder.HasSequence<int>("WarehouseSequence", schema);
        }

        public override int SaveChanges()
          => SaveChanges(acceptAllChangesOnSuccess: true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyMyEntityOverrides();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyMyEntityOverrides();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private static void ConfigureSoftDeleteFilter(ModelBuilder builder)
        {
            foreach (var softDeletableTypeBuilder in builder.Model.GetEntityTypes()
                .Where(x => typeof(ISoftDeletable).IsAssignableFrom(x.ClrType)))
            {
                var parameter = Expression.Parameter(softDeletableTypeBuilder.ClrType, "p");

                softDeletableTypeBuilder.SetQueryFilter(
                    Expression.Lambda(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt)),
                            Expression.Constant(null)),
                        parameter)
                );
            }
        }

        /// <summary>
        /// Automatically stores metadata when entities are added, modified, or deleted.
        /// </summary>
        private void ApplyMyEntityOverrides()
        {
            foreach (var entry in ChangeTracker.Entries<IAudited>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(nameof(IAudited.CreatedBy)).CurrentValue = _currentUser.UserId;
                        entry.Property(nameof(IAudited.CreatedAt)).CurrentValue = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(IAudited.LastModifiedBy)).CurrentValue = _currentUser.UserId;
                        entry.Property(nameof(IAudited.LastModifiedAt)).CurrentValue = _dateTime.Now;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged; // Override removal. Better than Modified, because that flags ALL properties for update.
                        entry.Property(nameof(ISoftDeletable.DeletedBy)).CurrentValue = _currentUser.UserId;
                        entry.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = _dateTime.Now;
                        break;
                }
            }
        }
    }
}