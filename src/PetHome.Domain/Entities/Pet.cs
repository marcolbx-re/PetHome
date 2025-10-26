namespace PetHome.Domain;

public abstract class Pet : BaseEntity
{
	public string Name { get; protected set; }
	public string Breed { get; protected set; }
	public PetType Type { get; protected set; }
	public DateTime BirthDate { get; protected set; }
	public DateTime RegistrationDate { get; protected set; } 
	public ICollection<Photo>? Photos {get;set;}
	public string SpecialInstructions { get; protected set; }
	//Foreign Key
	public Guid OwnerId { get; protected set; }
	public Owner Owner { get; protected set; }
	public GenderType Gender { get; protected set; }
    
	private readonly List<Stay> _stays = new();
	public IReadOnlyCollection<Stay> Stays => _stays.AsReadOnly();
	public bool RequiresSpecialDiet { get; protected set; }

	protected Pet() { } // EF Core

	protected Pet(string name, string breed, DateTime birthDate,
		Guid ownerId, GenderType gender, bool requiresSpecialDiet, string specialInstructions = "")
	{
		Id = Guid.NewGuid();
		Name = name;
		Breed = breed;
		BirthDate = birthDate;
		OwnerId = ownerId;
		SpecialInstructions = specialInstructions;
		Gender = gender;
		RequiresSpecialDiet = requiresSpecialDiet;
	}

	public abstract string GetPetType();
	public abstract decimal GetDailyRate();
}
public enum GenderType
{
	Male = 1,
	Female = 2
}

public enum PetType
{
	Dog = 1,
	Cat = 2,
}