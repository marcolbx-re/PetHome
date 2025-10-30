using AutoMapper;
using PetHome.Application.DTOs;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Pets.GetPet;
using PetHome.Application.Pets.PatchPet;
using PetHome.Application.Photos.GetPhoto;
using PetHome.Domain;

namespace PetHome.Application.Core;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Pet, PetDTO>();
		CreateMap<Photo, PhotoResponse>();
		CreateMap<Pet, PetSimpleResponse>();
		CreateMap<Domain.Owner, OwnerResponse>();
		
		// From domain to patch DTO
		CreateMap<Pet, PetPatchRequest>().ReverseMap();
	}
}
