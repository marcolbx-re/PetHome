using System.Text.Json.Serialization;
using PetHome.Application;
using PetHome.Application.Interfaces;
using PetHome.Infrastructure;
using PetHome.Infrastructure.Photos;
using PetHome.Infrastructure.Reports;
using PetHome.Persistence;
using PetHome.WebApi.Extensions;
using PetHome.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddPoliciesServices();
builder.Services
    .Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped(typeof(IReportService<>), typeof(ReportService<>));
builder.Services.AddInfrastructure();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerDocumentation();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("corsapp");
//app.UseHttpsRedirection();
await app.SeedDataAuthentication();
await app.SeedOwnersAsync();
await app.SeedPetsAsync();
app.MapControllers();
app.Run();