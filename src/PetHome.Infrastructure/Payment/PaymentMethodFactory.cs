using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class PaymentMethodFactory : IPaymentMethodFactory
{
	private readonly IServiceProvider _provider; // For DI

	public PaymentMethodFactory(IServiceProvider provider)
	{
		_provider = provider;
	}

	public IPaymentMethod Create(string paymentType)
	{
		return paymentType.ToLower() switch
		{
			"card" => _provider.GetRequiredService<CardPayment>(),
			"paypal" => _provider.GetRequiredService<PayPalPayment>(),
			"cash" => _provider.GetRequiredService<CashPayment>(),
			_ => throw new NotSupportedException($"Payment type {paymentType} is not supported")
		};
	}
}
