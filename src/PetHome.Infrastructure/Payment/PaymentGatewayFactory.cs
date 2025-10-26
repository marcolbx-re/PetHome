using PetHome.Application.Factories;
using PetHome.Application.Interfaces;

namespace PetHome.Infrastructure.Payment;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
	private readonly Dictionary<string, IPaymentGateway> _gateways;

	public PaymentGatewayFactory()
	{
		_gateways = new Dictionary<string, IPaymentGateway>(StringComparer.OrdinalIgnoreCase);
	}

	public IPaymentGateway GetGateway(string paymentType)
	{
		return _gateways.TryGetValue(paymentType, out var gateway) ? gateway : null;
	}

	public void RegisterGateway(IPaymentGateway gateway)
	{
		_gateways[gateway.PaymentType] = gateway;
	}
}