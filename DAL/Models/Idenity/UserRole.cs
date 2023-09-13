using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;


namespace DAL.Models.Idenity
{
	/// <summary>
	/// UserRole
	/// </summary>
	//[Validator(typeof(UserRoleValidator))]
	public class UserRole : IdentityUserRole<Guid>
	{
		#region Parents

		/// <summary>
		/// User
		/// </summary>
		public virtual User User
		{
			get;
			set;
		}

		/// <summary>
		/// Role
		/// </summary>
		public virtual Role Role
		{
			get;
			set;
		}

		#endregion
	}

	/// <summary>
	/// UserRole Validator
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class UserRoleValidator : AbstractValidator<UserRole>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UserRoleValidator()
		{
			RuleFor(m => m.UserId)
				.NotEmpty();

			RuleFor(m => m.RoleId)
				.NotEmpty();
		}
	}

	/// <summary>
	/// Entity configuration
	/// </summary>
	public sealed class UserRoleEntityTypeConfiguration : ICustomEntityTypeConfiguration<UserRole>
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		public void Configure(EntityTypeBuilder<UserRole> typeBuilder, ModelBuilder modelBuilder)
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
			modelBuilder.Ignore<IdentityUserRole<string>>();

			// Table
			typeBuilder.ToTable("UserRoles");

            // Keys, Unique Keys and Indices
            typeBuilder.HasKey(m => new { m.UserId, m.RoleId });

			// Fields
			//typeBuilder.Property(m => m.UserId)
			//	.IsRequired();

			//typeBuilder.Property(m => m.RoleId)
			//	.IsRequired();

			// Parents
			typeBuilder.HasOne(m => m.User)
				.WithMany(m=>m.UserRoles)
				.HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade);

			typeBuilder.HasOne(m => m.Role)
				.WithMany(m=>m.UserRoles)
				.HasForeignKey(c => c.RoleId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
