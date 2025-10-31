using PetHome.Domain;

namespace PetHome.Application.Stays.PostStay;

public class StayCreateRequest
{
	//public Guid PetId { get; set; }
	public DateTime CheckInDate { get; set; }
	public DateTime CheckOutDate { get; set; }
	public Stay.StayStatus Status { get; set; }
	public decimal DailyRate { get; set; }
	public decimal TotalCost { get; set; }
	public string? Notes { get; set; }
}
