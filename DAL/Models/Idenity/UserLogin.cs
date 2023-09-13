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
	/// UserLogin
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
	//[Validator(typeof(UserLoginValidator))]
	public class UserLogin : IdentityUserLogin<Guid>
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
	}

	/// <summary>
	/// UserLogin Validator
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class UserLoginValidator : AbstractValidator<UserLogin>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UserLoginValidator()
		{
			RuleFor(m => m.UserId)
				.NotEmpty();

			RuleFor(m => m.LoginProvider)
				.NotEmpty()
				.Length(0, ValidationConstants.ShortTextLength);

			RuleFor(m => m.ProviderKey)
				.NotEmpty()
				.Length(0, ValidationConstants.BigTextLength);

			RuleFor(m => m.ProviderDisplayName)
				.NotEmpty()
				.Length(0, ValidationConstants.NormalTextLength);
		}
	}

	/// <summary>
	/// Entity configuration
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
	public sealed class UserLoginEntityTypeConfiguration : ICustomEntityTypeConfiguration<UserLogin>
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		public void Configure(EntityTypeBuilder<UserLogin> typeBuilder, ModelBuilder modelBuilder)
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
			modelBuilder.Ignore<IdentityUserLogin<Guid>>();

			// Table
			typeBuilder.ToTable("UserLogins");

			// Keys, Unique Keys and Indices
			typeBuilder.HasKey(m => new { m.LoginProvider, m.ProviderKey });

			// Fields
			typeBuilder.Property(m => m.UserId)
				.IsRequired();

			typeBuilder.Property(m => m.LoginProvider)
				.IsRequired()
				.HasMaxLength(ValidationConstants.ShortTextLength);

			typeBuilder.Property(m => m.ProviderKey)
				.IsRequired()
				.HasMaxLength(ValidationConstants.BigTextLength);

			typeBuilder.Property(m => m.ProviderDisplayName)
				.IsRequired()
				.HasMaxLength(ValidationConstants.NormalTextLength);

			// Parents
			typeBuilder.HasOne(m => m.User)
				.WithMany(m => m.Logins)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
