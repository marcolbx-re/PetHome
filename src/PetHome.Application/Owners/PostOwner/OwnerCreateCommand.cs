using FluentValidation;
using MediatR;
using PetHome.Application.Core;
using PetHome.Persistence;

namespace PetHome.Application.Owner.OwnerCreate;

public class OwnerCreateCommand
{
	public record OwnerCreateCommandRequest(OwnerCreateRequest OwnerCreateRequest)
		: IRequest<Result<Guid>>, ICommandBase;
	
	internal class OwnerCreateCommandHandler
    : IRequestHandler<OwnerCreateCommandRequest, Result<Guid>>
    {
        private readonly PetHomeDbContext _context;

        public OwnerCreateCommandHandler(
            PetHomeDbContext context
            )
        {
            _context = context;
        }
        
        public async Task<Result<Guid>> Handle(
            OwnerCreateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            var dto = request.OwnerCreateRequest;
            var owner = new Domain.Owner(dto.FirstName, dto.LastName, dto.Email,
                dto.PhoneNumber, dto.IsNewsletterSubscribed, dto.IdentificationType, dto.IdentificationNumber);
            _context.Add(owner);

            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
            return resultado 
                        ? Result<Guid>.Success(owner.Id)
                        : Result<Guid>.Failure("No se pudo insertar el Owner");
        }
    }
    
    public class OwnerCreateCommandRequestValidator
        : AbstractValidator<OwnerCreateCommandRequest>
    {
        public OwnerCreateCommandRequestValidator()
        {
            RuleFor(x => x.OwnerCreateRequest).SetValidator(new OwnerCreateValidator());
        }

    }
}
