//using ApiSpotify.Services;
//using Microsoft.Extensions.Configuration;
//using ApiSpotify.ENDPOINTS;

//WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//// Configuració
//builder.Configuration
//    .SetBasePath(AppContext.BaseDirectory)
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
//DatabaseConnection dbConn = new DatabaseConnection(connectionString);

//WebApplication webApp = builder.Build();

//// Registra els endpoints en un mètode separat
//webApp.MapCancoEndpoints(dbConn);

//webApp.Run();

using ApiSpotify.Services;
using ApiSpotify.ENDPOINTS;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);



// Configuració
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Afegim Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Spotify",
        Version = "v1",
        Description = "API minimal per gestionar cançons, usuaris i llistes de reproducció."
    });
});

// Connexió a la base de dades
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseConnection dbConn = new DatabaseConnection(connectionString);

WebApplication webApp = builder.Build();

//webApp.UseAntiforgery();

// Activem Swagger només en desenvolupament
if (webApp.Environment.IsDevelopment())
{
    webApp.UseSwagger();
    webApp.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Spotify v1");
        options.RoutePrefix = string.Empty; // opcional: swagger a la arrel
    });
}

// Registra tots els endpoints (els fitxers han d'existir o ser placeholders)
webApp.MapCancoEndpoints(dbConn);
webApp.MapUsuarisEndpoints(dbConn);
webApp.MapLlistesReproduccionsEndpoints(dbConn);
webApp.MapLlistareproduccioCancoEndpointsEndpoints(dbConn);
webApp.MapQualitatsFitxersEndpoints(dbConn);
webApp.MapExtreureMetadadesEndpoints(dbConn);

webApp.Run();

