using PetHome.Domain;

namespace PetHome.Application.Interfaces;

public interface IReportService<T> where T : BaseEntity
{
	Task<MemoryStream> GetCsvReport(List<T> records);
}
