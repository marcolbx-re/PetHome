using PetHome.Application.DTOs;
using PetHome.Application.Interfaces;
using PetHome.Domain;

namespace PetHome.Application.Factories;

public class CatFactory : IPetFactory<CatCreationDTO, Cat>
{
	public Cat Create(CatCreationDTO dto)
	{
		Cat cat = new Cat
		(dto.Name, dto.Breed, dto.BirthDate, dto.OwnerId,
			dto.IsDeclawed, dto.RequiresSpecialDiet, dto.Gender,
			dto.SpecialInstructions
		);

		//TODO add Photos
		return cat;
	}
}
