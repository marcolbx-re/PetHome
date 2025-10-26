using PetHome.Application;
using PetHome.Infrastructure;
using PetHome.Persistence;
using PetHome.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerDocumentation();
var app = builder.Build();
app.useSwaggerDocumentation();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.Run();
