namespace PetHome.Application.Interfaces;

public interface IPaymentMethod
{
	Task<PaymentResult> PayAsync(decimal amount, string currency);
}

public class PaymentResult
{
	public bool Success { get; set; }
	public string TransactionId { get; set; }
	public string ErrorMessage { get; set; }
}
