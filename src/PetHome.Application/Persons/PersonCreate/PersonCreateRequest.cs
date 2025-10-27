namespace PetHome.Application.Persons.PersonCreate;

public class PersonCreateRequest
{
	public string? FirstName { get; set; } = null!;
	public string? LastName { get; set; } = null!;
	public int Age { get; set; } = 1;
}
