using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Yasy.Core.Interfaces;
using Yasy.Infrastructure;
using Yasy.Infrastructure.Data;
using Yasy.Server.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=yasy;Username=yasy;Password=yasy";
var dataPath = builder.Configuration["DataPath"]
    ?? Path.Combine(builder.Environment.ContentRootPath, "data");
builder.Services.AddInfrastructure(connectionString, dataPath);

var app = builder.Build();

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<YasyDbContext>();
    await db.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

// Serve profile images
app.MapGet("/api/images/{*path}", (string path, IProfileImageService imageService) =>
{
    var fullPath = imageService.GetAbsolutePath(path);
    if (!File.Exists(fullPath))
        return Results.NotFound();
    var contentType = Path.GetExtension(fullPath).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        _ => "application/octet-stream"
    };
    return Results.File(fullPath, contentType);
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
