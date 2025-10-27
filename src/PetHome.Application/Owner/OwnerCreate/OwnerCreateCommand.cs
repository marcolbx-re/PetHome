// using MediatR;
// using PetHome.Application.Core;
// using PetHome.Persistence;
//
// namespace PetHome.Application.Owner.OwnerCreate;
//
// public class OwnerCreateCommand
// {
// 	public record OwnerCreateCommandRequest(OwnerCreateDTO ownerCreateDTO)
// 		: IRequest<Result<Guid>>, ICommandBase;
// 	
// 	internal class OwnerCreateCommandHandler
//     : IRequestHandler<OwnerCreateCommandRequest, Result<Guid>>
//     {
//         private readonly PetHomeDbContext _context;
//
//         public OwnerCreateCommandHandler(
//             PetHomeDbContext context
//             )
//         {
//             _context = context;
//         }
//         
//         public async Task<Result<Guid>> Handle(
//             OwnerCreateCommandRequest request, 
//             CancellationToken cancellationToken
//         )
//         {
//             
//             var dto = request.ownerCreateDTO;
//             var owner = new Domain.Owner(dto.FirstName, dto.LastName, dto.Email,
//                 dto.PhoneNumber, dto.IsNewsletterSubscribed);
//
//             if(request.ownerCreateDTO.PetId is not null)
//             {
//                 var instructor =   _context.Pets!
//                 .FirstOrDefault(x => x.Id == request.ownerCreateDTO.PetId);
//
//                 if(instructor is null)
//                 {
//                     return Result<Guid>.Failure("No se encontro el Pet");
//                 }
//
//                 //owner.Pets = new List<Pet> {instructor};
//             }
//
//             // if(request.cursoCreateRequest.PrecioId is not null)    
//             // {
//             //     var precio = await _context.Precios!
//             //     .FirstOrDefaultAsync(x => x.Id == request.cursoCreateRequest.PrecioId);
//             //     
//             //     if(precio is null)
//             //     {
//             //         return Result<Guid>.Failure("No se encontro el precio");
//             //     }
//             //
//             //     curso.Precios = new List<Precio> {precio};
//             // }
//
//
//
//             _context.Add(owner);
//
//             var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
//          
//
//             return resultado 
//                         ? Result<Guid>.Success(owner.Id)
//                         : Result<Guid>.Failure("No se pudo insertar el Owner");
//         }
//     }
// }
