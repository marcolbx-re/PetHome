using PetHome.Domain;

namespace PetHome.Application.DTOs;

public record DogDTO : PetDTO
{
	public Size Size { get; set; }
}
