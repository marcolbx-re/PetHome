using PetHome.Domain;

namespace PetHome.Application.DTOs;

public abstract class PetCreationDTO
{
	public string Name { get; set; }
	public string Breed { get; set; }
	public DateTime BirthDate { get; set; }
	//public ICollection<Photo>? Photos {get;set;}
	public string SpecialInstructions { get; set; }
	public GenderType Gender { get; set; }
	public bool RequiresSpecialDiet { get; set; }
	public Guid OwnerId { get; set; }
	
}
