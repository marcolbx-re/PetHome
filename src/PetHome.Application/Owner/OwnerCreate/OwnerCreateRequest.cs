using System.Text.Json.Serialization;
using PetHome.Domain;

namespace PetHome.Application.Owner.OwnerCreate;

public class OwnerCreateRequest
{
	public string? FirstName { get; set; } = null!;
	public string? LastName { get; set; } = null!;
	public string? Email { get; set; } = null!;
	public string? PhoneNumber { get; set; } = null!;
	public bool IsNewsletterSubscribed { get;  set; }
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public IdentificationType IdentificationType { get; set; }
	public string? IdentificationNumber { get; set; } = null!;
}
