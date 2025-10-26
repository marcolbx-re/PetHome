using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class CardPayment : IPaymentMethod
{

	public Task<PaymentResult> PayAsync(decimal amount, string currency)
	{
		return Task.FromResult(new PaymentResult { Success = true, TransactionId = "CARD123" });
	}
}
