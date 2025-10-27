using System.Text.Json.Serialization;
using PetHome.Application;
using PetHome.Infrastructure;
using PetHome.Persistence;
using PetHome.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
Console.WriteLine("0000000000000000000");
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerDocumentation();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.UseInlineDefinitionsForEnums(); // optional
// });
builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
var app = builder.Build();
app.useSwaggerDocumentation();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }
app.UseCors("corsapp");
//app.UseHttpsRedirection();
app.MapControllers();
app.Run();
