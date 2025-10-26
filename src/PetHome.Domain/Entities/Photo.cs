namespace PetHome.Domain;

public class Photo : BaseEntity
{
	public string? Url {get;set;}
    
	public Guid? PetId {get;set;}
	public Pet? Pet {get;set;}

	public string? PublicId {get;set;}
}
