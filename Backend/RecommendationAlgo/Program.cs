using Common.MBcontracts;
using MassTransit;
using RecommendationAlgo.MessageConsumers;

var builder = WebApplication.CreateBuilder(args);


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();





app.Run();

