using PetHome.Application.Core;
using PetHome.Domain;

namespace PetHome.Application.Owner.GetOwners;

public class GetOwnersRequest : PagingParams
{
	public string? FirstName {get;set;}
	public string? LastName {get;set;}
	public string? IdentificationNumber {get;set;}
	public IdentificationType? IdentificationType {get;set;}
}
