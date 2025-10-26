using System.Collections;

namespace PetHome.Domain.Repositories;

public interface ITransactionRepository
{
	Task GetByIdAsync(Guid id, CancellationToken ct = default);
	Task<IEnumerable> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
	Task<Transaction> AddAsync(Transaction transaction);
	Task UpdateAsync(Transaction transaction);
	Task<IEnumerable<Transaction>> GetAllAsync();
}
