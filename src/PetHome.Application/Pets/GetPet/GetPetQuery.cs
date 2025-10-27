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

public class PetResponse
{
	public Guid? Id { get; set; }
	public string Name { get; set; } = null!;
	public string Breed { get; set; } = null!;
	public PetType PetType;
	public DateTime BirthDate;
	private DateTime RegistrationDate;
	private string SpecialInstructions;
	private Guid OwnerId;
	private Domain.Owner Owner;
	private GenderType Gender;
	private bool RequiresSpecialDiet;
}

// public class PetResponse(
// 	Guid? Id ,
// 	string Name,
// 	string Breed,
// 	PetType PetType,
// 	DateTime BirthDate,
// 	DateTime RegistrationDate,
// 	string SpecialInstructions,
// 	Guid OwnerId,
// 	Domain.Owner Owner,
// 	GenderType Gender,
// 	bool RequiresSpecialDiet
//
// // private readonly List<Stay> _stays = new();
// // public IReadOnlyCollection<Stay> Stays => _stays.AsReadOnly();
// );

