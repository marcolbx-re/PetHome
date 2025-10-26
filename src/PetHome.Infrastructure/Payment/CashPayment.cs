using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class CashPayment : IPaymentMethod
{

	public Task<PaymentResult> PayAsync(decimal amount, string currency)
	{
		return Task.FromResult(new PaymentResult { Success = true, TransactionId = "Cash1234" });
	}
}
