using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistance.Configurations;

public sealed class CarConfiguration : IEntityTypeConfiguration<Car>
{
	public void Configure(EntityTypeBuilder<Car> builder)
	{
		builder.ToTable("Cars"); //Database' de görülecek tablol ismi
		builder.HasKey(p => p.Id); // Id kolonunu Primary Key olduğun belirttik
		builder.HasIndex(p => p.Name);
	}
}
