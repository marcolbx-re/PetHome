﻿namespace PetHome.Domain;

public class Transaction : BaseEntity
{
	public Guid? StayId { get; protected set; }
	public Stay? Stay { get; protected set; }
	public decimal? Amount { get; protected set; }
	public PaymentMethod? PaymentMethod { get; private set; }
	public DateTime? CreatedAt { get; private set; }
	public TransactionStatus? Status { get; private set; }
	public string? Reference { get; private set; }
	//public Dictionary<string, string> Metadata { get; private set; }

	protected Transaction() { } // EF Core

	protected Transaction(Guid stayId, decimal amount)
	{
		Id = Guid.NewGuid();
		StayId = stayId;
		Amount = amount;
		Status = TransactionStatus.Pending;
		CreatedAt = DateTime.UtcNow;
	}

	public Transaction(decimal amount, PaymentMethod paymentMethod, Stay stay)
	{
		if (amount <= 0)
			throw new ArgumentException("Amount must be greater than zero", nameof(amount));

		Id = Guid.NewGuid();
		Amount = amount;
		PaymentMethod = paymentMethod;
		CreatedAt = DateTime.UtcNow;
		StayId = stay.Id;
		Stay = stay;
		Status = TransactionStatus.Pending;
		//Metadata = new Dictionary<string, string>();
	}

	public void MarkAsCompleted()
	{
		Status = TransactionStatus.Completed;
	}

	public void MarkAsFailed()
	{
		Status = TransactionStatus.Failed;
	}

	// public void AddMetadata(string key, string value)
	// {
	// 	Metadata[key] = value;
	// }

}

public enum TransactionStatus
{
	Pending,
	Completed,
	Failed,
	Refunded
}

public enum PaymentMethod
{
	NotSet = 0,
	Cash = 1,
	Card = 2,
	Paypal = 3
}