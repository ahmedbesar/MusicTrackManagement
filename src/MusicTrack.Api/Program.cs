using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MusicTrack.Application.Extensions;
using MusicTrack.Infrastructure.Data;
using MusicTrack.Infrastructure.Extensions;

const string UiCorsPolicy = "UiCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MusicTrack API",
        Version = "v1",
        Description = "Track Management API for a music distribution company."
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(UiCorsPolicy, policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(UiCorsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MusicTrackDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();
    await MusicTrackDbSeed.SeedAsync(context, logger);
}

app.Run();
