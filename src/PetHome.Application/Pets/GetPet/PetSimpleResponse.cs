using PetHome.Application.Photos.GetPhoto;
using PetHome.Domain;

namespace PetHome.Application.Pets.GetPet;

public class PetSimpleResponse
{
	public Guid? Id { get; set; }
	public string? Name { get; set; }
	public string? Breed { get; set; }
	public PetType? Type { get; set; }
	public List<PhotoResponse>? Photos {get; set;}
}
