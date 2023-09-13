using Core;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class DealerBroker: BaseEntity
    {
        public int Id { get; set; }
        public Guid BrokerId { get; set; }

        public User Broker { get; set; }
        public Guid DealerId { get; set; }

        public User Dealer { get; set; }
    }
    /// <summary>
    /// Entity configuration
    /// </summary>
    public class DealerBrokerTypeConfiguration : ICustomEntityTypeConfiguration<DealerBroker>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="typeBuilder">Type builder</param>
        /// <param name="modelBuilder">Model builder</param>
        public void Configure(EntityTypeBuilder<DealerBroker> typeBuilder, ModelBuilder modelBuilder)
        {
            if (typeBuilder == null)
            {
                throw new ArgumentNullException(nameof(typeBuilder));
            }
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            // Ignore base entity
            modelBuilder.Ignore<IdentityUser<Guid>>();


            // Table
            typeBuilder.ToTable("DealerBrokers");

            // Keys, Unique Keys and Indices
            typeBuilder.HasKey(m => m.Id);
            typeBuilder.HasKey(m =>new { m.BrokerId,m.DealerId});

            typeBuilder.HasOne<User>(m => m.Broker).WithMany(m => m.Brokers).HasForeignKey(m => m.BrokerId).OnDelete(DeleteBehavior.NoAction);

            typeBuilder.HasOne<User>(m => m.Dealer).WithMany(m => m.Dealers).HasForeignKey(m => m.DealerId).OnDelete(DeleteBehavior.NoAction);

            // Fields


            // Parents
        }
    }
}
