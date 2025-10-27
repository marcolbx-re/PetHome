using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Domain;
using PetHome.Persistence;
using PetHome.Persistence.Test;

namespace PetHome.Application.Persons.PersonCreate;

public class PersonCreateCommand
{
	public record PersonCreateCommandRequest(PersonCreateRequest personCreateRequest) 
    : IRequest<Result<Guid>>, ICommandBase;


    internal class PersonCreateCommandHandler
    : IRequestHandler<PersonCreateCommandRequest, Result<Guid>>
    {
        private readonly PetHomeDbContext _context;

        public PersonCreateCommandHandler(PetHomeDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<Guid>> Handle(
            PersonCreateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            
            var id = Guid.NewGuid();
            var person = new Person {
                Id = id,
                FirstName = request.personCreateRequest.FirstName,
                LastName = request.personCreateRequest.LastName,
                Age = request.personCreateRequest.Age,
            };
            
            _context.Add(person);

            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
         

            return resultado 
                        ? Result<Guid>.Success(person.Id)
                        : Result<Guid>.Failure("No se pudo insertar el person");
            
        }
    }


    public class PersonCreateCommandRequestValidator
    : AbstractValidator<PersonCreateCommandRequest>
    {
        public PersonCreateCommandRequestValidator()
        {
            //RuleFor(x => x.personCreateRequest).SetValidator(new CursoCreateValidator());
        }

    }

}
