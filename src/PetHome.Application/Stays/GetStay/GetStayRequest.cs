using PetHome.Application.Core;
using PetHome.Domain;

namespace PetHome.Application.Stays.GetStay;

public class GetStayRequest: PagingParams
{
	public DateTime? CheckInDate { get; set; }
	public DateTime? CheckOutDate { get; set; }
	public Stay.StayStatus? Status { get; set; }
	public PetType? Type {get; set;}
}