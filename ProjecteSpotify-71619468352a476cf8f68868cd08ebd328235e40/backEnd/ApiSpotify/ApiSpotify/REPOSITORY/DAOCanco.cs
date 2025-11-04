using ApiSpotify.MODELS;
using ApiSpotify.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSpotify.REPOSITORY
{
    public class DAOCanco
    {
        public static void Insert(DatabaseConnection dbConn, Canco canco)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Songs (Id, titol, artista, album, durada)
                          VALUES (@Id, @titol, @artista, @album, @durada)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", canco.Id);
            cmd.Parameters.AddWithValue("@titol", canco.Titol);
            cmd.Parameters.AddWithValue("@artista", canco.Artista);
            cmd.Parameters.AddWithValue("@album", canco.Album);
            cmd.Parameters.AddWithValue("@durada", canco.Durada);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");
            dbConn.Close();
        }

        public static List<Canco> GetAll(DatabaseConnection dbconn) 
        {
            List<Canco> canco = new();

            dbconn.Open();

            string sql = "SELECT Id, titol, artista, album, durada FROM Songs";

            using SqlCommand cmd = new SqlCommand(sql, dbconn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                canco.Add(new Canco
                {
                    Id = reader.GetGuid(0),
                    Titol = reader.GetString(1),
                    Artista = reader.GetString(2),
                    Album = reader.GetString(3),
                    Durada = reader.GetDecimal(4),
                });
            }

            dbconn.Close();
            return canco;
        }

        public static Canco GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();
            string sql = "SELECT Id, titol, artista, album, durada FROM Songs";

            using SqlCommand cmd = new SqlCommand( sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader(); 
            Canco? canco = null;

            if (reader.Read())
            {
                canco = new Canco
                {
                    Id = reader.GetGuid(0),
                    Titol = reader.GetString(1),
                    Artista = reader.GetString(2),
                    Album = reader.GetString(3),
                    Durada = reader.GetDecimal(4),
                };
            };

            dbConn.Close();
            return canco;
        }

        public static void Update(DatabaseConnection dbConn, Canco canco)
        {
            dbConn.Open();

            string sql = @"UPDATE Songs
                           SET titol = @titol,
                               artista = @artista,
                               album = @album,
                               durada = @durada
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", canco.Id);
            cmd.Parameters.AddWithValue("@titol", canco.Titol);
            cmd.Parameters.AddWithValue("@artista", canco.Artista);
            cmd.Parameters.AddWithValue("@album", canco.Album);
            cmd.Parameters.AddWithValue("@durada", canco.Durada);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Songs WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }
    }
}
