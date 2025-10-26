namespace PetHome.Domain;

public class Stay : BaseEntity
{
    //Foreign Key
    public Guid PetId { get; private set; }
    public Pet Pet { get; private set; }
    public DateTime CheckInDate { get; private set; }
    public DateTime CheckOutDate { get; private set; }
    public StayStatus Status { get; private set; }
    public decimal DailyRate { get; private set; }
    public decimal TotalCost { get; private set; }
    public string Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid? PaymentId { get; private set; }
    public Transaction Transaction { get; private set; }
    
    private readonly List<CareActivity> _careActivities = new();
    public IReadOnlyCollection<CareActivity> CareActivities => _careActivities.AsReadOnly();

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
    
    public void AddCareActivity(CareActivityType type, string performedBy, string notes = "")
    {
        _careActivities.Add(new CareActivity(Id, type, performedBy, notes));
    }

    public void AssignPayment(Guid paymentId) => PaymentId = paymentId;
}
