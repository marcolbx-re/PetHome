using PetHome.Application.DTOs;
using PetHome.Application.Interfaces;
using PetHome.Domain;

namespace PetHome.Application.Factories;

public class DogFactory : IPetFactory<DogCreateRequest, Dog>
{
	public Dog Create(DogCreateRequest dto)
	{
		Dog dog = new Dog
		(dto.Name, dto.Breed, dto.BirthDate, dto.OwnerId,
			dto.Size, dto.RequiresExtraExercise, dto.Gender, dto.RequiresSpecialDiet,
			dto.SpecialInstructions
		);

		//TODO add Photos
		return dog;
	}
}
