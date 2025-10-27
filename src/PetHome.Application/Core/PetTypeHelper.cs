using PetHome.Domain;

namespace PetHome.Application.Core;

public static class PetTypeHelper
{
	public static PetType Parse(string value)
	{
		return Enum.TryParse<PetType>(value, true, out var parsed)
			? parsed
			: PetType.Dog; // default/fallback
	}
}
