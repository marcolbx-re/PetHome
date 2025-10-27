namespace PetHome.Domain;

public class Cat : Pet
{
	public bool IsDeclawed { get; private set; }

	private Cat() { } // EF Core

	public Cat(string name, string breed, DateTime birthDate, 
		Guid ownerId, bool isDeclawed, bool requiresSpecialDiet, GenderType gender,
		string specialInstructions = "")
		: base(name, breed, birthDate, ownerId, gender, requiresSpecialDiet, PetType.Cat, specialInstructions)
	{
		IsDeclawed = isDeclawed;
		RequiresSpecialDiet = requiresSpecialDiet;
	}

	public override string GetPetType() => "Cat";

	public override decimal GetDailyRate()
	{
		decimal baseRate = 25.00m;
		return RequiresSpecialDiet ? baseRate + 5.00m : baseRate;
	}
}
