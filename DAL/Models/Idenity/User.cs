using Core;
using DAL.Models.Common;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Entity;

namespace DAL.Models.Idenity
{
    public class User : IdentityUser<Guid>
    {

        public User()
        {
            Id = Guid.NewGuid();
        }
        public string FullName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => LockoutEnabled && LockoutEnd >= DateTimeOffset.UtcNow;
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsSuperAdmin { get; set; }
        public int LocalOrder { get; set; }

        ///// <summary>
        ///// Navigation property for the roles this user belongs to.
        ///// </summary>
        public  ICollection<UserRole>  _roles = new List<UserRole>();

        public virtual ICollection<UserRole> UserRoles
        {
            get => _roles;
            set => _roles = (value ?? new List<UserRole>()).Where(p => p != null).ToList();
        }

        private ICollection<UserClaim> _claims = new List<UserClaim>();
        /// <summary>
        /// Claims
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserClaim> Claims
        {
            get => _claims;
            set => _claims = (value ?? new List<UserClaim>()).Where(p => p != null).ToList();
        }

        private ICollection<UserLogin> _logins = new List<UserLogin>();
        /// <summary>
        /// Logins
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLogin> Logins
        {
            get => _logins;
            set => _logins = (value ?? new List<UserLogin>()).Where(p => p != null).ToList();
        }

        private ICollection<UserToken> _tokens = new List<UserToken>();
        /// <summary>
        /// Tokens
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserToken> Tokens
        {
            get => _tokens;
            set => _tokens = (value ?? new List<UserToken>()).Where(p => p != null).ToList();
        }
        public IList<DealerBroker> Brokers { get; set; }
        public IList<DealerBroker> Dealers { get; set; }

        public IList<Notification> Notifications { get; set; }


    }
    /// <summary>
    /// User Validator
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class UserValidator : AbstractValidator<User>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty();


            RuleFor(m => m.Email)
                .NotEmpty()
                .Length(ValidationConstants.MinEmailLength, ValidationConstants.MaxEmailLength)
                .Matches(ValidationConstants.EmailRegexPattern);

         
        }
    }

    /// <summary>
    /// Entity configuration
    /// </summary>
    public  class UserEntityTypeConfiguration : ICustomEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="typeBuilder">Type builder</param>
        /// <param name="modelBuilder">Model builder</param>
        public void Configure(EntityTypeBuilder<User> typeBuilder, ModelBuilder modelBuilder)
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
            typeBuilder.ToTable("Users");

            // Keys, Unique Keys and Indices
            typeBuilder.HasKey(m => m.Id);
            typeBuilder.HasIndex(m => m.NormalizedUserName).IsUnique();
            typeBuilder.HasIndex(m => m.Email).IsUnique();
            typeBuilder.HasIndex(m => m.NormalizedEmail).IsUnique();
            typeBuilder.HasIndex(m => m.PhoneNumber).IsUnique();

            // Fields
            typeBuilder.Property(m => m.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(ValidationConstants.GuidLength);

            typeBuilder.Property(m => m.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxEmailLength);

            typeBuilder.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxEmailLength);

            typeBuilder.Property(m => m.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxEmailLength);

            typeBuilder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxPhoneNumberLength);

            // Parents
        }
    }
}
