using PetHome.Application.DTOs;

namespace PetHome.Application.Interfaces;

public interface IPaymentService
{
	Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
	Task<IEnumerable<TransactionCreationDTO>> GetTransactionHistoryAsync();
}
