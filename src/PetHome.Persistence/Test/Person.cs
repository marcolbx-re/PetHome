using PetHome.Domain;

namespace PetHome.Persistence.Test;

public class Person : BaseEntity
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public int Age { get; set; }
}
