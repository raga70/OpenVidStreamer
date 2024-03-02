using Account.Repository.EFC;
using Account.Services;
using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


var serverVersion = new MariaDbServerVersion(new Version(10, 4, 24));
builder.Services.AddDbContext<DatabaseContext>(options=>options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),serverVersion));

//Service DI
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<StripePaymentService>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.UseHttpsRedirection();

ConsulRegisterer.Register(app, app.Environment, app.Lifetime, builder.Configuration);



app.Run();

