using PetHome.Application.Core;

namespace PetHome.Application.Persons;

public class GetPersonsRequest : PagingParams
{

	public string? FirstName {get;set;}
	public string? LastName {get;set;}

}
