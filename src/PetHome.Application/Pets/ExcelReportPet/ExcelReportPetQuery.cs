using MediatR;
using Microsoft.EntityFrameworkCore;
using PetHome.Application.Interfaces;
using PetHome.Domain;
using PetHome.Persistence;

namespace PetHome.Application.Pets.ExcelReportPet;

public class ExcelReportPetQuery
{
	public record ExcelReportPetQueryRequest 
		: IRequest<MemoryStream>;

	internal class ExcelReportPetQueryHandler
		: IRequestHandler<ExcelReportPetQueryRequest, MemoryStream>
	{
		private readonly PetHomeDbContext _context;
		private readonly IReportService<Pet> _reporteService;

		public ExcelReportPetQueryHandler(
			PetHomeDbContext context, 
			IReportService<Pet> reporteService
		)
		{
			_context = context;
			_reporteService = reporteService;
		}

		public async Task<MemoryStream> Handle(
			ExcelReportPetQueryRequest request, 
			CancellationToken cancellationToken
		)
		{
			var pets = await _context.Pets!.Take(10).Skip(0)
				.Include(p => p.Owner).ToListAsync(cancellationToken: cancellationToken);

			return await _reporteService.GetCsvReport(pets);
		}
	}
}
