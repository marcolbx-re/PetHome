using System.ComponentModel.DataAnnotations.Schema;

namespace PetHome.Domain;

public enum IdentificationType
{
	DNI,
	NIE
}

// Domain/Entities/Owner.cs
public class Owner : BaseEntity
{
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string Email { get; private set; }
	public string PhoneNumber { get; private set; }
	public bool IsNewsletterSubscribed { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public IdentificationType IdentificationType { get; set; }
	public string IdentificationNumber { get; set; } = null!;
    
	private readonly List<Pet>? _pets = new();
	public IReadOnlyCollection<Pet>? Pets => _pets.AsReadOnly();
	
	[NotMapped]
    
	private readonly List<Transaction>? _transactions = new();
	[NotMapped]
	public IReadOnlyCollection<Transaction>? Transactions => _transactions.AsReadOnly();

	private Owner() { } // EF Core

	public Owner(string firstName, string lastName, string email, 
		string phoneNumber, bool isNewsletterSubscribed, IdentificationType identificationType, string identificationNumber)
	{
		Id = Guid.NewGuid();
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		PhoneNumber = phoneNumber;
		IsNewsletterSubscribed = isNewsletterSubscribed;
		CreatedAt = DateTime.UtcNow;
		IdentificationType = identificationType;
		IdentificationNumber = identificationNumber;
	}

	public void SubscribeToNewsletter() => IsNewsletterSubscribed = true;
	public void UnsubscribeFromNewsletter() => IsNewsletterSubscribed = false;
}