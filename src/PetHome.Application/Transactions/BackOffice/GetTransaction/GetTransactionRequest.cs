using PetHome.Application.Core;
using PetHome.Domain;

namespace PetHome.Application.DTOs.BackOffice.GetTransaction;

public class GetTransactionRequest : PagingParams
{
	public Guid? StayId { get; set; }
	public decimal? Amount { get; set; }
	public PaymentMethod? PaymentMethod { get; set; }
	public DateTime? CreatedAt { get; set; }
	public TransactionStatus? Status { get; set; }
	public string? Reference { get; set; }
}
