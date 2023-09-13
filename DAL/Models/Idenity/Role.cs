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
using System.Threading.Tasks;
using DAL.Models.Entity;

namespace DAL.Models.Idenity
{
    public class Role : IdentityRole<Guid>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Role"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public Role()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Role"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public Role(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the description for this role.
        /// </summary>
        public string Description { get; set; }

        public bool IsAllowDelete { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        #region Children

        private ICollection<RoleClaim> _claims = new List<RoleClaim>();
        /// <summary>
        /// Claims
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<RoleClaim> Claims
        {
            get => _claims;
            set => _claims = (value ?? new List<RoleClaim>()).Where(p => p != null).ToList();
        }

        #endregion

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles{ get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        //public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }
    /// <summary>
    /// Role Validator
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class RoleValidator : AbstractValidator<Role>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty();

            RuleFor(m => m.Name)
                .NotEmpty()
                .Length(ValidationConstants.MinRoleNameLength, ValidationConstants.MaxRoleNameLength)
                .Matches(ValidationConstants.RoleNameRegexPattern)
                .WithMessage("Please enter valid role name");

            RuleFor(m => m.Description)
                .Length(0, ValidationConstants.LargeTextLength);
        }
    }

    /// <summary>
    /// Entity configuration
    /// </summary>
    public sealed class RoleEntityTypeConfiguration : ICustomEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="typeBuilder">Type builder</param>
        /// <param name="modelBuilder">Model builder</param>
        public void Configure(EntityTypeBuilder<Role> typeBuilder, ModelBuilder modelBuilder)
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
            modelBuilder.Ignore<IdentityRole<Guid>>();

            // Table
            typeBuilder.ToTable("Roles");

            // Keys, Unique Keys and Indices
            typeBuilder.HasKey(m => m.Id);
            typeBuilder.HasIndex(m => m.NormalizedName).IsUnique();

            // Fields
            typeBuilder.Property(m => m.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(ValidationConstants.GuidLength);

            typeBuilder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxRoleNameLength);

            typeBuilder.Property(m => m.NormalizedName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.MaxRoleNameLength);

            typeBuilder.Property(m => m.Description)
                .HasMaxLength(ValidationConstants.LargeTextLength);
        }
    }
}
