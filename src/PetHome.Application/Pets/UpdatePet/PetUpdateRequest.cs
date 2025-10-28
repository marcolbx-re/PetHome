using PetHome.Domain;

namespace PetHome.Application.Pets.UpdatePet;

public class PetUpdateRequest
{
	public string? Name { get; set; }
	public string? Breed { get; set; }
	public string? PetType { get; set; }
	public DateTime? BirthDate { get; set; }
	public string? SpecialInstructions { get; set; }
	public Guid? OwnerId { get; set; }
	public GenderType? Gender { get; set; }
	public PetType? Type { get; set; }
	public bool? RequiresSpecialDiet { get; set; }
	public ICollection<Photo>? Photos {get; set;}
	public bool? IsDeclawed { get; set; }
	public Size? Size { get; set; }
}
