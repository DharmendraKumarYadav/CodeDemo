using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
	/// <summary>
	/// Entity type builder configuration marker interface
	/// </summary>
	public interface ICustomEntityTypeConfiguration
	{
	}

	/// <summary>
	/// Entity type builder configuration
	/// </summary>
	public interface ICustomEntityTypeConfiguration<TEntity> : ICustomEntityTypeConfiguration
		where TEntity: class
	{
		/// <summary>
		/// Configures the entity type
		/// </summary>
		/// <param name="typeBuilder">Type builder</param>
		/// <param name="modelBuilder">Model builder</param>
		void Configure(EntityTypeBuilder<TEntity> typeBuilder, ModelBuilder modelBuilder);
	}
}
