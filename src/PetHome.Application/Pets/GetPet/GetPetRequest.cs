using PetHome.Application.Core;

namespace PetHome.Application.Pets.GetPet;

public class GetPetRequest : PagingParams
{
	public string? Name {get; set;}
	public string? Breed {get; set;}
}
