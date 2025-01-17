using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using TestsContainers.Repository;

public sealed class SqlServerContainer : IAsyncLifetime
{
	private readonly MsSqlContainer _container;
	private const string SA_PASSWORD = "Password@123";
	 
	private OrderDbContext _dbContext = default!;

	public SqlServerContainer()
	{
		_container = new MsSqlBuilder()
			.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
			.WithPassword(SA_PASSWORD)
			.WithPortBinding(1433, true)
			.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
			.Build();
	}

	public string ConnectionString => _container.GetConnectionString();

	public async Task InitializeAsync()
	{
		await _container.StartAsync();
		await Task.Delay(4000); 
		await InitializeDatabaseAsync();
	}

	public async Task DisposeAsync() => await _container.DisposeAsync();

	private async Task InitializeDatabaseAsync()
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync();
		
		var options = new DbContextOptionsBuilder<OrderDbContext>()
			.UseSqlServer(connection)
			.Options;
		
		_dbContext = new OrderDbContext(options);
		
		await _dbContext.Database.EnsureCreatedAsync();
	}
	
}