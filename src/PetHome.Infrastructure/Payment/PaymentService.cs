using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class PaymentService
{
	private readonly IPaymentMethodFactory _factory;

	public PaymentService(IPaymentMethodFactory factory)
	{
		_factory = factory;
	}

	public async Task<PaymentResult> ProcessPayment(decimal amount, string currency, string paymentType)
	{
		var paymentMethod = _factory.Create(paymentType);
		return await paymentMethod.PayAsync(amount, currency);
	}
}
