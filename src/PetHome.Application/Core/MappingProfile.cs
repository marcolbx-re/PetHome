using AutoMapper;
using PetHome.Application.DTOs;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Pets.GetPet;
using PetHome.Domain;

namespace PetHome.Application.Core;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Pet, PetDTO>();
		CreateMap<Pet, PetSimpleResponse>();
		CreateMap<Domain.Owner, OwnerResponse>();
	}
}
