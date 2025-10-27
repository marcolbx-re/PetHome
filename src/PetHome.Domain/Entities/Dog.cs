namespace PetHome.Domain;

public class Dog : Pet
{
	public DogSize Size { get; private set; }
	public bool RequiresExtraExercise { get; private set; }
	
	public Dog(string name, string breed, DateTime birthDate, 
		Guid ownerId, DogSize size, bool requiresExtraExercise, GenderType gender,
		bool requiresSpecialDiet,
		string specialInstructions = "")
		: base(name, breed, birthDate, ownerId, gender,requiresSpecialDiet, PetType.Dog, specialInstructions)
	{
		Size = size;
		RequiresExtraExercise = requiresExtraExercise;
		RegistrationDate = DateTime.Now;
	}

	public Dog()
	{
		
	}

	public override string GetPetType() => "Dog";

	public override decimal GetDailyRate()
	{
		decimal baseRate = Size switch
		{
			DogSize.Small => 30.00m,
			DogSize.Medium => 40.00m,
			DogSize.Large => 50.00m,
			_ => 40.00m
		};

		return RequiresExtraExercise ? baseRate + 10.00m : baseRate;
	}
	
	
}

public enum DogSize
{
	Small,
	Medium,
	Large
}
