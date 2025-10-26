using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Interfaces;
using PetHome.Infrastructure.Payment;

namespace PetHome.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services
	)
	{
		// Register payment strategies
		services.AddTransient<CardPayment>();
		services.AddTransient<PayPalPayment>();
		services.AddTransient<CashPayment>();

		// Register factory
		services.AddSingleton<IPaymentMethodFactory, PaymentMethodFactory>();

// Register payment service
		services.AddTransient<PaymentService>();
		return services;
	}
}
