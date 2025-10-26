using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Interfaces;
using PetHome.Domain;

namespace PetHome.Application.Factories;

public class PetFactoryProvider
{
	private readonly IServiceProvider _serviceProvider;
	public PetFactoryProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public IPetFactory<TDto, TEntity> GetFactory<TDto, TEntity>() where TDto : class where TEntity : Pet
	{
		var factory = _serviceProvider.GetService<IPetFactory<TDto, TEntity>>()
		           ?? throw new NotSupportedException("No factory for {typeof(TDto).Name} -> {typeof(TEntity).Name}");
		return factory;
	}
}
