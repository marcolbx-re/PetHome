using PetHome.Application.Core;

namespace PetHome.Application.Owner.GetOwners;

public class GetOwnersRequest : PagingParams
{
	public string? FirstName {get;set;}
	public string? LastName {get;set;}
}
