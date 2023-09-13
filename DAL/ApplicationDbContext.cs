using Core;
using Core.Extension;
using DAL.Models.Idenity;
using DAL.Models.Common;
using DAL.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
        public string CurrentUserId { get; set; }

        #region Initial Tables

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// UserClaims
        /// </summary>
        public DbSet<UserClaim> UserClaims { get; set; }

        /// <summary>
        /// RoleClaims
        /// </summary>
        public DbSet<RoleClaim> RoleClaims { get; set; }

        /// <summary>
        /// UserLogins
        /// </summary>
        public DbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// UserTokens
        /// </summary>
        public DbSet<UserToken> UserTokens { get; set; }

        /// <summary>
        /// UserRoles
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Users
        /// </summary>
        public DbSet<UserProfile> UserProfiles { get; set; }



        #endregion
        public DbSet<Area> Area { get; set; }
        public DbSet<Attributes> Attributes { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<BikeCity> BikeCity { get; set; }
        public DbSet<BikeUserRequest> BikeUserRequests { get; set; }
        public DbSet<BikeUserRating> BikeUserRatings { get; set; }
        public DbSet<BikeColour> BikeColours { get; set; }
        public DbSet<BikeFeatures> BikeFeatures { get; set; }
        public DbSet<BikeNews> BikeNews { get; set; }
        public DbSet<BikePrice> BikePrice { get; set; }
        public DbSet<BikeSimilar> BikeSimilar { get; set; }
        public DbSet<BikeSpecifications> BikeSpecifications { get; set; }
        public DbSet<BikeVariant> BikeVariant { get; set; }
        public DbSet<BodyStyle> BodyStyles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Displacement> Displacements { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<FeaturedBike> FeaturedBikes { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<BikeImage> BikeImages { get; set; }
  
        public DbSet<Specification> Specifications { get; set; }

        public DbSet<BikeBooking> BikeBookings { get; set; }

        public DbSet<SaleBike> SaleBikes { get; set; }

        public DbSet<DealerBroker> DealerBrokers { get; set; }
        public DbSet<ShowRoom> ShowRooms { get; set; }
        public DbSet<DealerEmployee> DealerEmployees { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //const string priceDecimalType = "decimal(18,2)";

            //builder.Entity<User>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<Role>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            //base.OnModelCreating(builder);
            //builder.Ignore<IdentityUserRole<string>>();

            // Table
            // Table
            //  builder.Entity<UserRole>().HasNoKey();
            //builder.Entity<UserRole>().ToTable("UserRoles");
            //builder.Entity<UserRole>().HasKey(m => new { m.UserId, m.RoleId });

            //ConfigureEntity<User, UserEntityTypeConfiguration>(builder);
            //ConfigureEntity<Role, RoleEntityTypeConfiguration>(builder);
            //ConfigureEntity<RoleClaim, RoleClaimEntityTypeConfiguration>(builder);
            //ConfigureEntity<UserClaim, UserClaimEntityTypeConfiguration>(builder);
            //ConfigureEntity<UserLogin, UserLoginEntityTypeConfiguration>(builder);
            //ConfigureEntity<UserRole, UserRoleEntityTypeConfiguration>(builder);
            //ConfigureEntity<UserProfile, UserProfileEntityTypeConfiguration>(builder);
            //ConfigureEntity<UserToken, UserUserTokenEntityTypeConfiguration>(builder);
            // This
   
            var configureEntityMethod = typeof(ApplicationDbContext)
                .GetMethod("ConfigureEntity", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var entityConfigurationsAssembly = typeof(ICustomEntityTypeConfiguration).GetTypeInfo().Assembly;
            var allConfiguratorTypes = entityConfigurationsAssembly
                .GetLoadableTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsArray && !t.IsNested && typeof(ICustomEntityTypeConfiguration).IsAssignableFrom(t.AsType()))
                .ToList();

            GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.GetTypeInfo().IsGenericType && (p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)))
                .Select(p => p.PropertyType.GetGenericArguments()[0])
                .Select(p =>
                {
                    var configuratorType = allConfiguratorTypes.SingleOrDefault(
                        t => typeof(ICustomEntityTypeConfiguration<>).MakeGenericType(p).IsAssignableFrom(t.AsType()));
                    return (configuratorType != null) ? configureEntityMethod.MakeGenericMethod(p, configuratorType.AsType()) : null;
                })
                .Where(p => p != null)
                .ForEach(p => p.Invoke(this, new object[] { builder }));
        }

        private void ConfigureEntity<TEntity, TEntityConfigurationType>(ModelBuilder modelBuilder)
        where TEntity : class
        where TEntityConfigurationType : ICustomEntityTypeConfiguration<TEntity>, new()
        {
            var entityConfigurator = new TEntityConfigurationType();
            entityConfigurator.Configure(modelBuilder.Entity<TEntity>(), modelBuilder);
        }
 
        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
              
                
            }
        }
    }
}
