using PatientManagementService.Context;
using PatientManagementService.Interfaces;
using PatientManagementService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
//UserEntity
builder.Services.AddScoped<PatientInterface, PatientService>();

// Configure HttpClient
builder.Services.AddHttpClient("ExternalService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/api/User/GetUsersList"); // Replace with your external service base URL
    // Configure other HttpClient options if needed
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
