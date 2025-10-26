using PetHome.Domain;

namespace PetHome.Application.Interfaces;

public interface IPetFactory<in TDto, TEntity> where TDto : class where TEntity : Pet
{
	TEntity Create(TDto dto);
}
