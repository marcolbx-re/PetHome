using PetHome.Application.Interfaces;

namespace PetHome.Application.Factories;

public interface IPaymentGatewayFactory
{
	IPaymentGateway GetGateway(string paymentType);
	void RegisterGateway(IPaymentGateway gateway);
}


public class PaymentGatewayFactory
{
	
}
