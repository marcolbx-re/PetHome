namespace PetHome.Application.Pets.GetPet;

public class PetSimpleResponse
{
	public Guid? Id { get; set; }
	public string? Name { get; set; }
	public string? Breed { get; set; }
	public string? PetType { get; set; }
}
