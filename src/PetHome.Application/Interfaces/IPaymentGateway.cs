using PetHome.Application.DTOs;

namespace PetHome.Application.Interfaces;

public interface IPaymentGateway
{
	string PaymentType { get; }
	Task<PaymentGatewayResponse> ProcessAsync(decimal amount, Dictionary<string, string> paymentDetails);
}
