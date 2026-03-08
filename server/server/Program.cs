using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Endpoints;
using server.Extensions;

var builder = WebApplication.CreateBuilder(args); ; 

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddUseCases();
builder.Services.AddConverters();
builder.Services.AddInfrastructure();
builder.Services.AddDbContext<CustomersContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration["Client:Url"]!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomersContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapCustomersEndpoints();
app.UseCors("ClientPolicy");

app.Run();