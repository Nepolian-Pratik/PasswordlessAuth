using Microsoft.EntityFrameworkCore;
using PasswordlessAuth.Helpers;
using PasswordlessAuth.Models;
using PasswordlessAuth.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<ProjDbContext>(options =>
//options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DbConnection")));
//sqlServerOptionsAction: sqlOptions =>
//{
//    sqlOptions.EnableRetryOnFailure(
//        maxRetryCount: 5,
//        maxRetryDelay: TimeSpan.FromSeconds(10),
//        errorNumbersToAdd: null);
//}));
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<RSAEncryption>();
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