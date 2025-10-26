using MasterNet.Application.Core;
using MediatR;
using PetHome.Application.Core;
using PetHome.Domain;

namespace PetHome.Application.Pets.GetPet;

public class GetPetQuery
{
	public record GetPetQueryRequest
		: IRequest<Result<PagedList<PetResponse>>>
	{
		public GetPetRequest? PetRequest {get;set;}
	}
}

public record PetResponse(
	Guid? Id,
	string Name,
	string Breed,
	DateTime BirthDate,
	DateTime RegistrationDate, 
	string SpecialInstructions,
	Guid OwnerId,
	Domain.Owner Owner,
	GenderType Gender,
	bool RequiresSpecialDiet
    
// private readonly List<Stay> _stays = new();
// public IReadOnlyCollection<Stay> Stays => _stays.AsReadOnly();
);