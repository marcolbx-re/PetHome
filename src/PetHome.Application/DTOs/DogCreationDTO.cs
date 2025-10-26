using PetHome.Domain;

namespace PetHome.Application.DTOs;

public class DogCreationDTO : PetCreationDTO
{
	public DogSize Size { get; private set; }
	public bool RequiresExtraExercise { get; private set; }
}
