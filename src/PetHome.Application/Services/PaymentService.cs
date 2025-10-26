using PetHome.Application.DTOs;
using PetHome.Application.Factories;
using PetHome.Application.Interfaces;
using PetHome.Domain;
using PetHome.Domain.Repositories;

namespace PetHome.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGatewayFactory _gatewayFactory;

    public PaymentService(
        ITransactionRepository transactionRepository,
        IPaymentGatewayFactory gatewayFactory)
    {
        _transactionRepository = transactionRepository;
        _gatewayFactory = gatewayFactory;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        try
        {
            // Get appropriate payment gateway
            var gateway = _gatewayFactory.GetGateway(request.PaymentMethod);
            if (gateway == null)
            {
                return new PaymentResponse
                {
                    Success = false,
                    Message = $"Payment method '{request.PaymentMethod}' is not supported",
                    ErrorCode = "UNSUPPORTED_PAYMENT_METHOD"
                };
            }

            // Process payment through gateway
            var gatewayResponse = await gateway.ProcessAsync(request.Amount, request.PaymentDetails);

            // Create transaction
            var transaction = new Transaction(
                request.Amount,
                request.PaymentMethod,
                gatewayResponse.Reference
            );

            // Add metadata from gateway
            if (gatewayResponse.Metadata != null)
            {
                foreach (var metadata in gatewayResponse.Metadata)
                {
                    transaction.AddMetadata(metadata.Key, metadata.Value);
                }
            }

            // Update transaction status
            if (gatewayResponse.Success)
            {
                transaction.MarkAsCompleted();
            }
            else
            {
                transaction.MarkAsFailed();
            }

            // Save to database
            await _transactionRepository.AddAsync(transaction);

            return new PaymentResponse
            {
                Success = gatewayResponse.Success,
                Message = gatewayResponse.Message,
                TransactionId = transaction.Id,
                Reference = transaction.Reference,
                ErrorCode = gatewayResponse.ErrorCode
            };
        }
        catch (Exception ex)
        {
            return new PaymentResponse
            {
                Success = false,
                Message = "An error occurred processing the payment",
                ErrorCode = "PROCESSING_ERROR"
            };
        }
    }
    
    public async Task<IEnumerable<TransactionCreationDTO>> GetTransactionHistoryAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        return transactions.Select(t => new TransactionCreationDTO
        {
            Id = t.Id,
            Amount = t.Amount,
            PaymentMethod = t.PaymentMethod,
            Timestamp = t.CreatedAt,
            Status = t.Status.ToString(),
            Reference = t.Reference
        });
    }
}
