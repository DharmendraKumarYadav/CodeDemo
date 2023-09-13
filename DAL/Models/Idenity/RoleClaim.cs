using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Core;

namespace DAL.Models.Idenity
{
	/// <summary>
	/// RoleClaim
	/// </summary>
	//[Validator(typeof(RoleClaimValidator))]
	public class RoleClaim : IdentityRoleClaim<Guid>
	{
		#region Parent

		/// <summary>
		/// Role
		/// </summary>
		public virtual Role Role
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
	/// RoleClaim Validator
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class RoleClaimValidator : AbstractValidator<RoleClaim>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RoleClaimValidator()
		{
			RuleFor(m => m.Id)
				.NotEmpty();

			RuleFor(m => m.RoleId)
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
	public sealed class RoleClaimEntityTypeConfiguration : ICustomEntityTypeConfiguration<RoleClaim>
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		public void Configure(EntityTypeBuilder<RoleClaim> typeBuilder, ModelBuilder modelBuilder)
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
			modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

			// Table
			typeBuilder.ToTable("RoleClaims");

			// Keys, Unique Keys and Indices
			// ReSharper disable once AssignNullToNotNullAttribute
			typeBuilder.HasBaseType((string)null);
			typeBuilder.HasKey(m => m.Id);
			//typeBuilder.HasIndex(m => new { m.RoleId, m.ClaimType }).IsUnique();


			// Fields
			typeBuilder.Property(m => m.Id)
				.ValueGeneratedOnAdd();

			typeBuilder.Property(m => m.RoleId)
				.IsRequired();

			typeBuilder.Property(m => m.ClaimType)
				.IsRequired()
				.HasMaxLength(ValidationConstants.BigTextLength);

			typeBuilder.Property(m => m.ClaimValue)
				.HasMaxLength(ValidationConstants.LargeTextLength);

			typeBuilder.Property(m => m.ClaimValueType)
				.HasMaxLength(ValidationConstants.BigTextLength);

			typeBuilder.Property(m => m.ClaimIssuer)
				.HasMaxLength(ValidationConstants.BigTextLength);

			// Parents
			typeBuilder.HasOne(m => m.Role)
				.WithMany(m => m.Claims)
				.HasForeignKey(c => c.RoleId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
