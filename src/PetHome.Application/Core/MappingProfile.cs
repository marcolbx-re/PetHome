using AutoMapper;
using PetHome.Application.DTOs;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Persons;
using PetHome.Application.Pets.GetPet;
using PetHome.Domain;
using PetHome.Persistence.Test;

namespace PetHome.Application.Core;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Pet, PetResponse>()
			.ForMember(dest => dest.PetType,
				opt => 
					opt.MapFrom(src => PetTypeHelper.Parse(src.GetPetType())));

// Optional polymorphic mappings
		CreateMap<Pet, PetDTO>();
		CreateMap<Dog, DogDTO>().IncludeBase<Pet, PetDTO>();
		CreateMap<Cat, CatDTO>().IncludeBase<Pet, PetDTO>();
		CreateMap<Person, PersonResponse>();
		CreateMap<Domain.Owner, OwnerResponse>();
		// CreateMap<Precio, PrecioResponse>();
		//
		// CreateMap<Instructor, InstructorResponse>()
		// 	.ForMember(dest=> dest.Apellido, src=>src.MapFrom(doc => doc.Apellidos));
  //       
		// CreateMap<Calificacion, CalificacionResponse>()
		// 	.ForMember(dest=> dest.NombreCurso, src=>src.MapFrom(doc => doc.Curso!.Titulo));
	}
}
