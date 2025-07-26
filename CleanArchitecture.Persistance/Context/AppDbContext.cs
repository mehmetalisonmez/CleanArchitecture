using CleanArchitecture.Domain.Abstractions;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistance.Context;

public sealed class AppDbContext : DbContext, IUnitOfWork
{
	//AppDbContext context = new();
	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//	optionsBuilder.UseSqlServer("");
	//}

		// Üstteki yöntemden tek farkı Connection bilgisini appsettings.json' da tutabilmemiz	
	public AppDbContext(DbContextOptions options) : base(options){}
	protected override void OnModelCreating(ModelBuilder modelBuilder) =>
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly); // Bu tek satırlık kod ile artık 100 tane configuration' da olsa otomatik olarak AppDbContext' e bağlanmış olacak

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entires = ChangeTracker.Entries<Entity>(); //Entiy class'ına sahip olanları listeledik
		foreach (var entry in entires) // CreatedDate ve UpdatedDate alanları otomatik olarak dolacak
		{
			if(entry.State == EntityState.Added)
				entry.Property(p=> p.CreatedDate)
					.CurrentValue = DateTime.Now;

			if (entry.State == EntityState.Modified)
				entry.Property(p => p.UpdateDate)
					.CurrentValue = DateTime.Now;
		}
		return base.SaveChangesAsync(cancellationToken);
	}
}
