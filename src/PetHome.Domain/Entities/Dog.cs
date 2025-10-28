namespace PetHome.Domain;

public class Dog : Pet
{
	public Size Size { get; private set; }
	public bool RequiresExtraExercise { get; private set; }
	
	public Dog(string name, string breed, DateTime birthDate, 
		Guid ownerId, Size size, bool requiresExtraExercise, GenderType gender,
		bool requiresSpecialDiet,
		string specialInstructions = "")
		: base(name, breed, birthDate, ownerId, gender,requiresSpecialDiet, PetType.Dog, false, size, specialInstructions)
	{
		Size = size;
		RequiresExtraExercise = requiresExtraExercise;
		RegistrationDate = DateTime.Now;
	}

	public Dog()
	{
		
	}
	
	public override decimal GetDailyRate()
	{
		decimal baseRate = Size switch
		{
			Size.Small => 30.00m,
			Size.Medium => 40.00m,
			Size.Large => 50.00m,
			_ => 40.00m
		};

		return RequiresExtraExercise ? baseRate + 10.00m : baseRate;
	}
	
	
}

public enum Size
{
	Small,
	Medium,
	Large
}
