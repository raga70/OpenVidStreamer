using Account.Repository.EFC;
using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var serverVersion = new MariaDbServerVersion(new Version(10, 4, 24));
builder.Services.AddDbContext<DatabaseContext>(options=>options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),serverVersion));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ConsulRegisterer.Register(app, app.Environment, app.Lifetime, builder.Configuration);



app.Run();

