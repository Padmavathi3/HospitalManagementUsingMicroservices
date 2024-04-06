using AppointmentManagementService.Context;
using AppointmentManagementService.Interface;
using AppointmentManagementService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>(); 
builder.Services.AddScoped<AppointmentInterface,AppointmentService>();

// Configure HttpClient
builder.Services.AddHttpClient("ExternalService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7245/api/Doctor/GetDoctorBySpecialization"); // Replace with your external service base URL
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
