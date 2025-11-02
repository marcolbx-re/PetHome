namespace PetHome.Application.DTOs;

public class PaymentRequest
{
	public string PaymentMethod { get; set; }
	public decimal Amount { get; set; }
	public Dictionary<string, string> PaymentDetails { get; set; }
}

public class PaymentResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public Guid? TransactionId { get; set; }
	public string Reference { get; set; }
	public string ErrorCode { get; set; }
}
