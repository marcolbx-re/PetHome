using System.Collections;

namespace PetHome.Domain.Repositories;

public interface IStayRepository
{
	Task GetByIdAsync(Guid id, CancellationToken ct = default);
	Task<IEnumerable> GetActiveStaysAsync(CancellationToken ct = default);
	Task<IEnumerable> GetByPetIdAsync(Guid petId, CancellationToken ct = default);
	Task<IEnumerable> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default);
	Task AddAsync(Stay stay, CancellationToken ct = default);
	Task UpdateAsync(Stay stay, CancellationToken ct = default);
}
