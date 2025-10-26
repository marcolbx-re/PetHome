using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.DTOs;
using PetHome.Application.Factories;
using PetHome.Application.Interfaces;
using PetHome.Domain;

namespace PetHome.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(
		this IServiceCollection services
	)
	{
		// services.AddMediatR(configuration => {
		// 	configuration
		// 		.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
		//
		// 	configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
		// });
  //       
		// services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
		//
		// services.AddAutoMapper(typeof(MappingProfile).Assembly);

		services.AddScoped<IPetFactory<DogCreationDTO, Dog>, DogFactory>();
		services.AddScoped<IPetFactory<CatCreationDTO, Cat>, CatFactory>();
		services.AddScoped<PetFactoryProvider>();

		return services;
	}
}
