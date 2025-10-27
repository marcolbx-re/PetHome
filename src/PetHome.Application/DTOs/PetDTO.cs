using PetHome.Domain;

namespace PetHome.Application.DTOs;

public abstract record PetDTO
{
	public string Name { get; protected set; }
	public string Breed { get; protected set; }
	public DateTime BirthDate { get; protected set; }
	public ICollection<Photo>? Photos {get;set;}
	public string SpecialInstructions { get; protected set; }
	public Guid OwnerId { get; protected set; }
	public Domain.Owner Owner { get; protected set; }
	public GenderType Gender { get; protected set; }
	public bool RequiresSpecialDiet { get; protected set; }
}
