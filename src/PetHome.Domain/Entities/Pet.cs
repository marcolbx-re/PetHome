namespace PetHome.Domain;

public class Pet : BaseEntity
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
    
	private readonly List<Stay>? _stays = new();
	public IReadOnlyCollection<Stay>? Stays => _stays.AsReadOnly();
	public bool RequiresSpecialDiet { get; protected set; }
	public bool? IsDeclawed { get; protected set; }
	public Size? Size { get; private set; }

	protected Pet() { } // EF Core

	public Pet(string name, string breed, DateTime birthDate,
		Guid ownerId, GenderType gender, bool requiresSpecialDiet, PetType petType, bool isDeclawed,Size size, string specialInstructions = "" )
	{
		Id = Guid.NewGuid();
		Name = name;
		Breed = breed;
		BirthDate = birthDate;
		OwnerId = ownerId;
		SpecialInstructions = specialInstructions;
		Gender = gender;
		Type = petType;
		RequiresSpecialDiet = requiresSpecialDiet;
		IsDeclawed = isDeclawed;
		Size = size;
	}

	public string GetPetType()
	{
		return Type.ToString();
	}
	public virtual decimal GetDailyRate()
	{
		return 0;
	}
}
public enum GenderType
{
	Male = 1,
	Female = 2
}

public enum PetType
{
	None = 0,
	Dog = 1,
	Cat = 2,
}