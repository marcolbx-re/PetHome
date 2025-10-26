namespace PetHome.Domain;

public class PaymentResult
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public Transaction Transaction { get; set; }
	public string ErrorCode { get; set; }
}
