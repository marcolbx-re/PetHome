using System.Text.Json.Serialization;

namespace PetHome.Domain;

public class Pet : BaseEntity
{
	public string? Name { get; set; }
	public string? Breed { get; set; }
	public PetType? Type { get; set; }
	public DateTime? BirthDate { get; set; }
	public DateTime? RegistrationDate { get; set; } 
	public ICollection<Photo>? Photos {get;set;}
	public string? SpecialInstructions { get; set; }
	//Foreign Key
	public Guid? OwnerId { get; set; }
	public Owner? Owner { get; set; }
	public GenderType? Gender { get; set; }
    
	private readonly List<Stay>? _stays = new();
	public IReadOnlyCollection<Stay>? Stays => _stays.AsReadOnly();
	public bool? RequiresSpecialDiet { get; set; }
	public bool? IsDeclawed { get; set; }
	public Size? Size { get; set; }

	protected Pet() { } // EF Core

	public Pet(string name, string breed, DateTime birthDate,
		Guid ownerId, GenderType gender, bool requiresSpecialDiet, PetType type, bool isDeclawed,Size size, string specialInstructions = "" )
	{
		Id = Guid.NewGuid();
		Name = name;
		Breed = breed;
		BirthDate = birthDate;
		OwnerId = ownerId;
		SpecialInstructions = specialInstructions;
		Gender = gender;
		Type = type;
		RequiresSpecialDiet = requiresSpecialDiet;
		IsDeclawed = isDeclawed;
		Size = size;
		RegistrationDate = DateTime.Now;
	}
	public virtual decimal GetDailyRate()
	{
		return 0;
	}
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GenderType
{
	Male = 1,
	Female = 2
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PetType
{
	None = 0,
	Dog = 1,
	Cat = 2,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Size
{
	Small = 0,
	Medium = 1,
	Big = 2,
}