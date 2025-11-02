
using System.Text.Json.Serialization;

namespace PetHome.Domain;

public class Stay : BaseEntity
{
    //Foreign Key
    public Guid? PetId { get; set; }
    public Pet? Pet { get;  set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get;  set; }
    public StayStatus? Status { get; set; }
    public decimal? DailyRate { get; set; }
    public decimal? TotalCost { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Transaction? Transaction { get; set; }

    private Stay() { } // EF Core

    public Stay(Guid petId, DateTime checkInDate, DateTime checkOutDate, decimal dailyRate)
    {
        Id = Guid.NewGuid();
        PetId = petId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Status = StayStatus.Scheduled;
        DailyRate = dailyRate;
        CreatedAt = DateTime.UtcNow;
        
        var days = (checkOutDate.Date - checkInDate.Date).Days;
        if (days < 1) days = 1;
        TotalCost = dailyRate * days;
    }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StayStatus
    {
        Scheduled,
        Active,
        Completed,
        Cancelled
    }

    public void CheckIn()
    {
        if (Status != StayStatus.Scheduled)
            throw new InvalidOperationException("Can only check in scheduled stays");
        Status = StayStatus.Active;
    }

    public void CheckOut()
    {
        if (Status != StayStatus.Active)
            throw new InvalidOperationException("Can only check out active stays");
        Status = StayStatus.Completed;
    }

    public void Cancel()
    {
        if (Status == StayStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed stays");
        Status = StayStatus.Cancelled;
    }
}
