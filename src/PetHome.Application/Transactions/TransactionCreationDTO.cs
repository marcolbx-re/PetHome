namespace PetHome.Application.DTOs;

public class TransactionCreationDTO
{
	public Guid Id { get; set; }
	public decimal Amount { get; set; }
	public string PaymentMethod { get; set; }
	public DateTime Timestamp { get; set; }
	public string Status { get; set; }
	public string Reference { get; set; }
}
