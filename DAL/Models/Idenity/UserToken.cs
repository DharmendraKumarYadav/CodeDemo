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
	/// UserToken
	/// </summary>
	//[Validator(typeof(UserTokenValidator))]
	public class UserToken : IdentityUserToken<Guid>
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
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class UserTokenValidator : AbstractValidator<UserToken>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UserTokenValidator()
		{
			RuleFor(m => m.UserId)
				.NotEmpty();

			RuleFor(m => m.LoginProvider)
				.NotEmpty()
				.Length(0, ValidationConstants.NormalTextLength);

			RuleFor(m => m.Name)
				.NotEmpty()
				.Length(0, ValidationConstants.NormalTextLength);
		}
	}

	/// <summary>
	/// Entity configuration
	/// </summary>
	public sealed class UserUserTokenEntityTypeConfiguration : ICustomEntityTypeConfiguration<UserToken>
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		public void Configure(EntityTypeBuilder<UserToken> typeBuilder, ModelBuilder modelBuilder)
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
			modelBuilder.Ignore<IdentityUserToken<Guid>>();

			// Table
			typeBuilder.ToTable("UserTokens");

			// Keys, Unique Keys and Indices
			typeBuilder.HasKey(m => new { m.UserId, m.LoginProvider, m.Name });

			// Fields
			typeBuilder.Property(m => m.UserId)
				.IsRequired();

			typeBuilder.Property(m => m.LoginProvider)
				.IsRequired()
				.HasMaxLength(ValidationConstants.NormalTextLength);

			typeBuilder.Property(m => m.Name)
				.IsRequired()
				.HasMaxLength(ValidationConstants.NormalTextLength);

			// Parents
			typeBuilder.HasOne(m => m.User)
				.WithMany(m => m.Tokens)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
