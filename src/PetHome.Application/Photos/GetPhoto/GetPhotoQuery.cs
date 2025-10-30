namespace PetHome.Application.Photos.GetPhoto;

public record PhotoResponse(
	Guid? Id,
	string? Url,
	Guid? PetId
)
{
	public PhotoResponse(): this(null, null, null)
	{
	}
}