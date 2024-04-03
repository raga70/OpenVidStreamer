using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using VideoStreamer;

var builder = WebApplication.CreateBuilder(args);

var consulIP = Environment.GetEnvironmentVariable("POD_IP");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(app.Configuration["PVstorageBucketPath"]),
    RequestPath = "/videos",
    ServeUnknownFileTypes = true, // Caution: serving unknown file types can be a security risk
    DefaultContentType = "application/octet-stream", // Set a default MIME type
    ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
    {
        { ".m3u8", "application/vnd.apple.mpegurl" },
        // Add any other file types not recognized by default
    })
});



ConsulRegisterer.Register(app, app.Environment, app.Lifetime, builder.Configuration);

app.Run();

