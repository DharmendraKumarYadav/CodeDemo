using System;
using System.Diagnostics.CodeAnalysis;
using Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Models.Idenity
{
	/// <summary>
	/// User
	/// </summary>
	//[Validator(typeof(UserProfileValidator))]
	public class UserProfile
	{
		#region Parents
		#endregion

		#region Properties

		/// <summary>
		/// User Id
		/// </summary>
		public Guid UserId
		{
			get;
			set;
		}

		/// <summary>
		/// Concurrency Token
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] ConcurrencyStamp
		{
			get;
			set;
		}

		/// <summary>
		/// Photo data
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] Photo
		{
			get;
			set;
		}

		/// <summary>
		/// Photo mime
		/// </summary>
		public string PhotoMime
		{
			get;
			set;
		}

		#endregion

		#region Children
		#endregion
	}

	/// <summary>
	/// User Validator
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class UserProfileValidator : AbstractValidator<UserProfile>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UserProfileValidator()
		{
			RuleFor(m => m.UserId)
				.NotEmpty();

			RuleFor(m => m.PhotoMime)
				.Length(0, ValidationConstants.TinyTextLength)
				.Must((m, p) => (m.Photo == null) || !string.IsNullOrWhiteSpace(p))
				.WithMessage("Photo is required");
		}
	}

	/// <summary>
	/// Entity configuration
	/// </summary>
	public sealed class UserProfileEntityTypeConfiguration : ICustomEntityTypeConfiguration<UserProfile>
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		public void Configure(EntityTypeBuilder<UserProfile> typeBuilder, ModelBuilder modelBuilder)
		{
			if (typeBuilder == null)
			{
				throw new ArgumentNullException(nameof(typeBuilder));
			}
			if (modelBuilder == null)
			{
				throw new ArgumentNullException(nameof(modelBuilder));
			}

			// Table
			typeBuilder.ToTable("UserProfiles");

			// Keys, Unique Keys and Indices
			typeBuilder.HasKey(m => m.UserId);

			// Fields
			typeBuilder.Property(m => m.ConcurrencyStamp)
				.HasColumnType("timestamp")
				.IsConcurrencyToken()
				.ValueGeneratedOnAddOrUpdate();

			typeBuilder.Property(m => m.Photo)
				.HasColumnType("varbinary(max)");

			typeBuilder.Property(m => m.PhotoMime)
				.HasMaxLength(ValidationConstants.TinyTextLength);

			// Parents
		}
	}
}
