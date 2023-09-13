using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using Core;

namespace DAL.Models.Idenity
{
    /// <summary>
    /// UserClaim
    /// </summary>
    //[Validator(typeof(UserClaimValidator))]
    public class UserClaim : IdentityUserClaim<Guid>
    {
        #region Parent

        /// <summary>
        /// User
        /// </summary>
        public virtual User User
        {
            get;
            set;
        }

        #endregion

        #region Properties

        private string _claimValueType;
        /// <summary>
        /// Claim value type
        /// </summary>
        public string ClaimValueType
        {
            get => _claimValueType;
            set => _claimValueType = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private string _claimIssuer;
        /// <summary>
        /// Claim issuer
        /// </summary>
        public string ClaimIssuer
        {
            get => _claimIssuer;
            set => _claimIssuer = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        #endregion
    }

    /// <summary>
    /// UserClaim Validator
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class UserClaimValidator : AbstractValidator<UserClaim>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserClaimValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty();

            RuleFor(m => m.UserId)
                .NotEmpty();

            RuleFor(m => m.ClaimType)
                .NotEmpty()
                .Length(0, ValidationConstants.BigTextLength);

            RuleFor(m => m.ClaimValue)
                .Length(0, ValidationConstants.LargeTextLength);

            RuleFor(m => m.ClaimValueType)
                .Length(0, ValidationConstants.BigTextLength);

            RuleFor(m => m.ClaimIssuer)
                .Length(0, ValidationConstants.BigTextLength);
        }
    }

    /// <summary>
    /// Entity configuration
    /// </summary>
    public sealed class UserClaimEntityTypeConfiguration : ICustomEntityTypeConfiguration<UserClaim>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="typeBuilder">Type builder</param>
        /// <param name="modelBuilder">Model builder</param>
        public void Configure(EntityTypeBuilder<UserClaim> typeBuilder, ModelBuilder modelBuilder)
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
            modelBuilder.Ignore<IdentityUserClaim<Guid>>();

            // Table
            typeBuilder.ToTable("UserClaims");

            // Keys, Unique Keys and Indices
            // ReSharper disable once AssignNullToNotNullAttribute
            typeBuilder.HasBaseType((string)null);
            typeBuilder.HasKey(m => m.Id);
            typeBuilder.HasIndex(m => new { m.UserId, m.ClaimType }).IsUnique();

            // Fields
            typeBuilder.Property(m => m.UserId)
                .IsRequired();

            typeBuilder.Property<int>("UserForeignKey");

            typeBuilder.Property(m => m.ClaimType)
                .IsRequired()
                .HasMaxLength(ValidationConstants.BigTextLength);

            typeBuilder.Property(m => m.ClaimValue)
                .HasMaxLength(ValidationConstants.LargeTextLength);

            typeBuilder.Property(m => m.ClaimValueType)
                .HasMaxLength(ValidationConstants.BigTextLength);

            typeBuilder.Property(m => m.ClaimIssuer)
                .HasMaxLength(ValidationConstants.BigTextLength);

            typeBuilder.HasOne(a => a.User)
    .WithMany(b => b.Claims)
    .HasForeignKey("UserForeignKey");

            // Parent
            typeBuilder.HasOne(m => m.User)
                .WithMany(m => m.Claims)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
