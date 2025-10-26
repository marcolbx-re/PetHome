namespace PetHome.Application.Owner.OwnerCreate;

public class OwnerCreateDTO
{
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string Email { get; private set; }
	public string PhoneNumber { get; private set; }
	public bool IsNewsletterSubscribed { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public Guid? PetId {get;set;}
}
