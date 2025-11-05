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
    public static class EndpointPerfils
    {
        public static void MapPerfilEndpoints(this WebApplication app, DatabaseConnection dbConn)
        {
            // GET ALL
            app.MapGet("/perfils", () =>
            {
                List<Perfil> perfils = DAOPerfils.GetAll(dbConn);
                return Results.Ok(perfils);
            });

            // GET BY ID
            app.MapGet("/perfils/{id}", (Guid id) =>
            {
                Perfil? perfil = DAOPerfils.GetById(dbConn, id);

                return perfil is not null
                    ? Results.Ok(perfil)
                    : Results.NotFound(new { message = $"Canco with Id {id} not found." });
            });

            // POST /cancons
            app.MapPost("/perfils", ([FromBody] PerfilRequest req) =>
            {
                Perfil perfil = new Perfil
                {
                    Id = Guid.NewGuid(),
                    Nom = req.Nom,
                    Descripcio = req.Descripcio,
                    Estat = req.Estat
                };

                DAOPerfils.Insert(dbConn, perfil);

                return Results.Created($"/perfils/{perfil.Id}", perfil);



            });

            app.MapPut("/perfils/{id}", (Guid id, PerfilRequest req) =>
            {
                var existing = DAOPerfils.GetById(dbConn, id);
                if (existing == null)
                    return Results.NotFound();

                Perfil updated = new Perfil
                {
                    Id = id,
                    Nom = req.Nom,
                    Descripcio = req.Descripcio,
                    Estat = req.Estat
                    
                };

                DAOPerfils.Update(dbConn, updated);
                return Results.Ok(updated);
            });

            app.MapDelete("/perfils/{id}", (Guid id) =>
                DAOPerfils.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        }
    }
}

// DTO pel request
public record PerfilRequest(String Nom, string Descripcio, string Estat);
