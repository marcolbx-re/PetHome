using PetHome.Domain;

namespace PetHome.Application.Pets.PatchPet;

public class PetPatchRequest
{
	public Guid? OwnerId { get; set; }  // Nullable, since not always present
	public string? Name { get; set; }
	public string? Breed { get; set; }
	public DateTime? BirthDate { get; set; }
	public string? SpecialInstructions { get; set; }
	public GenderType? Gender { get; set; }
	public PetType? Type { get; set; }
	public bool? RequiresSpecialDiet { get; set; }
	public bool? IsDeclawed { get; set; }
	public Size? Size { get; set; }
}
