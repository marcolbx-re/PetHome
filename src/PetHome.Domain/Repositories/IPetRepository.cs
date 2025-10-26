using System.Collections;

namespace PetHome.Domain.Repositories;

public interface IPetRepository
{
	Task GetByIdAsync(Guid id, CancellationToken ct = default);
	Task<IEnumerable> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
	Task AddAsync(Pet pet, CancellationToken ct = default);
	Task UpdateAsync(Pet pet, CancellationToken ct = default);
}
