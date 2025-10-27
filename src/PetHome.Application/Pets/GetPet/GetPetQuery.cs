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
	public string Name { get; set; }
	public string Breed { get; set; }
	public PetType PetType { get; set; }
	public DateTime BirthDate { get; set; }
	public DateTime RegistrationDate { get; set; }
	public string SpecialInstructions { get; set; }
	public Guid OwnerId { get; set; }
	//public Domain.Owner Owner { get; set; }
	public GenderType Gender { get; set; }
	public bool RequiresSpecialDiet { get; set; }
}

