namespace TestsContainers.Domain;

public class Order
{
	public Guid Id { get; set; }
	public Guid CustomerId { get; set; }
	public DateTime CreatedAt { get; set; }
}