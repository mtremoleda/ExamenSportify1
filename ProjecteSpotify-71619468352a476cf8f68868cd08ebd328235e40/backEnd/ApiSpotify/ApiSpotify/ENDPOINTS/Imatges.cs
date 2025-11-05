using ApiSpotify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiSpotify.MODELS;
using ApiSpotify.REPOSITORY;
using Microsoft.AspNetCore.Mvc;

namespace ApiSpotify.ENDPOINTS
{
    public static class EndpointImatges
    {
        public static void MapImatgesEndpoints(this WebApplication app, DatabaseConnection dbConn)
        {
            // GET ALL
            app.MapGet("/imatges", () =>
            {
                List<Imatge> imatge = DAOImatges.GetAll(dbConn);
                return Results.Ok(imatge);
            });

            // GET BY ID
            app.MapGet("/imatge/{id}", (Guid id) =>
            {
                Imatge? imatge = DAOImatges.GetById(dbConn, id);

                return imatge is not null
                    ? Results.Ok(imatge)
                    : Results.NotFound(new { message = $"Canco with Id {id} not found." });
            });

            // POST /cancons
            app.MapPost("/imatge", ([FromBody] ImatgeRequest req) =>
            {
                Imatge imatge = new Imatge
                {
                    Id = Guid.NewGuid(),
                    Titul = req.Titul,
                    Descripcio = req.Descripcio
                    
                };

                DAOImatges.Insert(dbConn, imatge);

                return Results.Created($"/imatges/{imatge.Id}", imatge);



            });

            app.MapPut("/imatges/{id}", (Guid id, ImatgeRequest req) =>
            {
                var existing = DAOImatges.GetById(dbConn, id);
                if (existing == null)
                    return Results.NotFound();

                Imatge updated = new Imatge
                {
                    Id = id,
                    Titul = req.Titul,
                    Descripcio = req.Descripcio
                    

                };

                DAOImatges.Update(dbConn, updated);
                return Results.Ok(updated);
            });

            app.MapDelete("/imatges/{id}", (Guid id) =>
                DAOImatges.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        }
    }
}

// DTO pel request
public record ImatgeRequest(String Titul, string Descripcio);
