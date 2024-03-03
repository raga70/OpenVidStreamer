using Common.MBcontracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary;
using RecommendationAlgo.MessageConsumers;
using RecommendationAlgo.Repository;
using RecommendationAlgo.Repository.EFC;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MariaDbServerVersion(new Version(10, 4, 24));
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion));

builder.Services.AddScoped<RecommendationRepository>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<VideoRecommendationsRequestConsumer>();
    busConfigurator.AddConsumer<AddWatchTimeToVideoRequestConsumer>();
    busConfigurator.AddConsumer<VideoMetadataConsumer>();
    busConfigurator.AddConsumer<DeleteUserRequestConsumer>();
    busConfigurator.AddConsumer<DeleteVideoRequestConsumer>();
    
    
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

app.MapControllers();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



ConsulRegisterer.Register(app, app.Environment, app.Lifetime, builder.Configuration);

app.Run();

