using ApiSpotify.MODELS;
using ApiSpotify.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ApiSpotify.REPOSITORY
{
    public class DAOLlistaReproduccioCanco
    {
        public static void Insert(DatabaseConnection dbConn, LlistaReproduccioCanco relacio)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Playlist_song (Id, id_song, id_playlist)
                           VALUES (@Id, @id_song, @id_playlist)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", relacio.Id);
            cmd.Parameters.AddWithValue("@id_song", relacio.IdCanco);
            cmd.Parameters.AddWithValue("@id_playlist", relacio.IdLlista);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");

            dbConn.Close();
        }

        public static List<LlistaReproduccioCanco> GetAll(DatabaseConnection dbConn)
        {
            List<LlistaReproduccioCanco> relacions = new();
            dbConn.Open();

            string sql = "SELECT Id, id_song, id_playlist FROM Playlist_song";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                relacions.Add(new LlistaReproduccioCanco
                {
                    Id = reader.GetGuid(0),
                    IdCanco = reader.GetGuid(1),
                    IdLlista = reader.GetGuid(2)
                });
            }

            dbConn.Close();
            return relacions;
        }

        public static LlistaReproduccioCanco? GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = "SELECT Id, id_song, id_playlist FROM Playlist_song WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            LlistaReproduccioCanco? relacio = null;

            if (reader.Read())
            {
                relacio = new LlistaReproduccioCanco
                {
                    Id = reader.GetGuid(0),
                    IdCanco = reader.GetGuid(1),
                    IdLlista = reader.GetGuid(2)
                };
            }

            dbConn.Close();
            return relacio;
        }

        public static void Update(DatabaseConnection dbConn, LlistaReproduccioCanco rel)
        {
            dbConn.Open();

            string sql = @"UPDATE Playlist_song
                           SET id_playlist = @id_playlist,
                               id_song = @id_song,
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", rel.Id);
            cmd.Parameters.AddWithValue("@id_song", rel.IdCanco);
            cmd.Parameters.AddWithValue("@id_playlist", rel.IdLlista);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }
        
        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Playlist_song WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }
    }
}
