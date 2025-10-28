using PetHome.Domain;

namespace PetHome.Application.Owner.UpdateOwner;

public class OwnerUpdateRequest
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public bool IsNewsletterSubscribed { get; set; }
	public IdentificationType IdentificationType { get; set; }
	public string? IdentificationNumber { get; set; }
}
