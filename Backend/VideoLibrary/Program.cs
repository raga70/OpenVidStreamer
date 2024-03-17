using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary;
using OpenVisStreamer.VideoLibrary.MessageConsumers;
using OpenVisStreamer.VideoLibrary.Repository;
using OpenVisStreamer.VideoLibrary.Repository.EFC;
using OpenVisStreamer.VideoLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MariaDbServerVersion(new Version(10, 4, 24))));

builder.Services.AddScoped<VideoRepository>();
builder.Services.AddScoped<VideoService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<UploadVideoRequestConsumer>();
    busConfigurator.AddConsumer<UpdateVideoToPublicRequestConsumer>();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["RabbitMQ:HostAddress"]!), h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        configurator.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();



ConsulRegisterer.Register(app, app.Environment, app.Lifetime, builder.Configuration);

app.Run();

