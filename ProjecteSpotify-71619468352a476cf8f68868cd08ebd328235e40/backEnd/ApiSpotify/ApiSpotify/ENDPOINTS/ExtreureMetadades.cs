using ApiSpotify.MODELS;
using ApiSpotify.REPOSITORY;
using ApiSpotify.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using TagLib;
using System.IO;
using System.Threading.Tasks;
using TagLib.Aac;

namespace ApiSpotify.ENDPOINTS
{
    public static class ExtreureMetadades
    {
        public static void MapExtreureMetadadesEndpoints(this WebApplication app, DatabaseConnection dbConn)
        {
            // POST /cancons/upload
            app.MapPost("/imatges/upload", async ([FromForm] IFormFileCollection files) =>
            {
                if (files == null || files.Count == 0)
                    return Results.BadRequest(new { message = "No s'ha rebut cap fitxer." });

                ConcurrentBag<Imatge> canconsProcessades = new ConcurrentBag<Imatge>();

                var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };

                // Transformem la col·lecció a una llista per a poder-la iterar en Parallel.ForEach
                var fileList = files.ToList();
                
                await Task.Run(() =>
                {
                    Parallel.ForEach(fileList, options, file =>
                    {
                        string tempPath = null;
                        try
                        {
                            var tempFileName = Path.GetRandomFileName();
                            tempPath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(tempFileName, ".mp3"));

                            using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                file.CopyTo(fs);
                            }

                            using (var tagFile = TagLib.File.Create(tempPath))
                            {
                                var tag = tagFile.Tag;
                                var props = tagFile.Properties;

                                var imatge = new Imatge
                                {
                                    Id = Guid.NewGuid(),
                                    Titul = string.IsNullOrWhiteSpace(tag.Title) ? Path.GetFileNameWithoutExtension(file.FileName) : tag.Title,
                                    Descripcio = string.IsNullOrWhiteSpace(tag.Comment) ? Path.GetFileNameWithoutExtension(file.FileName) : tag.Comment,

                                    
                                };

                                canconsProcessades.Add(imatge);

                                //TODO: Es podria guardar canço a la base de dades
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processant {file.FileName}: {ex.Message}");
                        }
                        
                    });
                });

                return Results.Ok(canconsProcessades);
            }).DisableAntiforgery();


        }

    }
}
