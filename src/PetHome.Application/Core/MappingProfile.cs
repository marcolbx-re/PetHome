using AutoMapper;
using PetHome.Application.DTOs;
using PetHome.Application.Owner.GetOwners;
using PetHome.Application.Pets.GetPet;
using PetHome.Application.Pets.PatchPet;
using PetHome.Application.Photos.GetPhoto;
using PetHome.Application.Stays.GetPetStayHistory;
using PetHome.Application.Stays.GetStay;
using PetHome.Application.Stays.GetStays;
using PetHome.Domain;

namespace PetHome.Application.Core;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Pet, PetDTO>();
		CreateMap<Pet, PetSimpleResponse>();
		CreateMap<Pet, PetPatchRequest>().ReverseMap();
		CreateMap<Photo, PhotoResponse>();
		CreateMap<Domain.Owner, OwnerResponse>();
		CreateMap<Stay, StayResponse>();
		CreateMap<Stay, StaySimpleResponse>();
		CreateMap<Stay, PetStayResponse>();
	}
}
