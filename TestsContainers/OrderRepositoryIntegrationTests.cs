using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TestsContainers.Domain;
using TestsContainers.Repository;
using Xunit;

namespace TestsContainers;

[Trait("Category", "Integration")]
[Trait("Container", "SqlServer")]
public sealed class OrderRepositoryIntegrationTests : IClassFixture<SqlServerContainer>
{
	private readonly SqlServerContainer _container;
	private readonly OrderRepository _orderRepository;

	public OrderRepositoryIntegrationTests(SqlServerContainer container)
	{
		_container = container;
		var options = new DbContextOptionsBuilder<OrderDbContext>()
			.UseSqlServer(_container.ConnectionString)
			.Options;

		var dbContext = new OrderDbContext(options);
		_orderRepository = new OrderRepository(dbContext);
	}


	[Fact]
	public async Task CreateAndGetOrder_ShouldPersistAndRetrieveOrder()
	{
		// Arrange
		var orderId = Guid.NewGuid();
		var order = new Order
		{
			Id = orderId,
			CustomerId = Guid.NewGuid(),
			CreatedAt = DateTime.UtcNow
		};

		// Act
		await _orderRepository.CreateAsync(order);
		var retrievedOrder = await _orderRepository.GetByIdAsync(orderId);

		// Assert
		Assert.NotNull(retrievedOrder);
		Assert.Equal(orderId, retrievedOrder.Id);
		Assert.Equal(order.CustomerId, retrievedOrder.CustomerId);
	}
}