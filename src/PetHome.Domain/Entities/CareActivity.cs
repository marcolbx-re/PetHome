namespace PetHome.Domain;

public class CareActivity : BaseEntity
{
	public Guid StayId { get; private set; }
	public Stay Stay { get; private set; }
	public CareActivityType Type { get; private set; }
	public DateTime PerformedAt { get; private set; }
	public string PerformedBy { get; private set; }
	public string Notes { get; private set; }

	private CareActivity() { } // EF Core

	public CareActivity(Guid stayId, CareActivityType type, string performedBy, string notes = "")
	{
		Id = Guid.NewGuid();
		StayId = stayId;
		Type = type;
		PerformedAt = DateTime.UtcNow;
		PerformedBy = performedBy;
		Notes = notes;
	}
}

public enum CareActivityType
{
	Feeding,
	WaterRefill,
	Petting,
	Grooming,
	Exercise,
	VetVisit
}
