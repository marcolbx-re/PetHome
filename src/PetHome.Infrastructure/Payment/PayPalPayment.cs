using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class PayPalPayment : IPaymentMethod
{

	public Task<PaymentResult> PayAsync(decimal amount, string currency)
	{
		return Task.FromResult(new PaymentResult { Success = true, TransactionId = "PAYPAL456" });
	}
}
