using Microsoft.EntityFrameworkCore;
using TestsContainers.Domain;

namespace TestsContainers.Repository;

public class OrderDbContext : DbContext
{
	public OrderDbContext(DbContextOptions<OrderDbContext> options)
		: base(options)
	{
	}

	public DbSet<Order> Orders => Set<Order>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasKey(o => o.Id);
		});
	}
}

public class OrderRepository
{
	private readonly OrderDbContext _dbContext;

	public OrderRepository(OrderDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task CreateAsync(Order order)
	{
		_dbContext.Orders.Add(order);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<Order?> GetByIdAsync(Guid id)
	{
		return await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
	}
}

