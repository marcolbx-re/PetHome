using Microsoft.AspNetCore.Mvc;
using PetHome.Domain;

namespace PetHome.Application.DTOs;

public class DogCreateRequest : PetCreationDTO
{
	[FromForm(Name = "Size")]
	public DogSize Size { get; set; }
	[FromForm(Name = "RequiresExtraExercise")]
	public bool RequiresExtraExercise { get; set; }
}
