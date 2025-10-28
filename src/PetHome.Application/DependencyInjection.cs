using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Application.Core;

namespace PetHome.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(
		this IServiceCollection services
	)
	{
		services.AddMediatR(configuration => {
			configuration
			.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
		
			configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});
         
		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
		
		services.AddAutoMapper(typeof(MappingProfile).Assembly);

		return services;
	}
}
