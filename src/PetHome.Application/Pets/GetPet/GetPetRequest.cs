using PetHome.Application.Core;
using PetHome.Domain;

namespace PetHome.Application.Pets.GetPet;

public class GetPetRequest : PagingParams
{
	public string? Name {get; set;}
	public string? Breed {get; set;}
	public PetType? Type {get; set;}
}
