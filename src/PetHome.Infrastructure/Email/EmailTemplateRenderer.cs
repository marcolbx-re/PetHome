namespace PetHome.Infrastructure.Email;

public static class EmailTemplateRenderer
{
	public static async Task<string> RenderAsync(string templateName, object model)
	{
		var templatePath = Path.Combine(AppContext.BaseDirectory, "Email",
			"Templates", $"{templateName}.html");
		var template = await File.ReadAllTextAsync(templatePath);
		var properties = model.GetType().GetProperties();
		foreach (var prop in properties)
		{
			var placeholder = $"{{{{{prop.Name}}}}}";
			var value = prop.GetValue(model)?.ToString() ?? "";
			template = template.Replace(placeholder, value);
		}
		return template;
	}
}
