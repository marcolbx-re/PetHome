using System.Collections;

namespace PetHome.Domain.Repositories;

public interface IOwnerRepository
{
	Task GetByIdAsync(Guid id, CancellationToken ct = default);
	Task GetByEmailAsync(string email, CancellationToken ct = default);
	Task<IEnumerable> GetNewsletterSubscribersAsync(CancellationToken ct = default);
	Task AddAsync(Owner owner, CancellationToken ct = default);
	Task UpdateAsync(Owner owner, CancellationToken ct = default);
}
