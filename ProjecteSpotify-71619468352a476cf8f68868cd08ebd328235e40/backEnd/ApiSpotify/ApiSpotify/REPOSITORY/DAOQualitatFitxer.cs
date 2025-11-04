using ApiSpotify.MODELS;
using ApiSpotify.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ApiSpotify.REPOSITORY
{
    public class DAOQualitatFitxer
    {
        public static void Insert(DatabaseConnection dbConn, QualitatFitxer fitxer)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Files_quality (Id, id_song, format, bitrate, mida)
                           VALUES (@Id, @id_song, @format, @bitrate, @mida)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", fitxer.Id);
            cmd.Parameters.AddWithValue("@id_song", fitxer.IdCanco);
            cmd.Parameters.AddWithValue("@format", fitxer.Format);
            cmd.Parameters.AddWithValue("@bitrate", fitxer.Bitrate);
            cmd.Parameters.AddWithValue("@mida", fitxer.Mida);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");

            dbConn.Close();
        }

        public static List<QualitatFitxer> GetAll(DatabaseConnection dbConn)
        {
            List<QualitatFitxer> fitxers = new();
            dbConn.Open();

            string sql = "SELECT Id, id_song, format, bitrate, mida FROM Files_quality";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                fitxers.Add(new QualitatFitxer
                {
                    Id = reader.GetGuid(0),
                    IdCanco = reader.GetGuid(1),
                    Format = reader.GetString(2),
                    Bitrate = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Mida = reader.IsDBNull(4) ? null : reader.GetDecimal(4)
                });
            }

            dbConn.Close();
            return fitxers;
        }

        public static QualitatFitxer? GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = "SELECT Id, id_song, format, bitrate, mida FROM Files_quality WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            QualitatFitxer? fitxer = null;

            if (reader.Read())
            {
                fitxer = new QualitatFitxer
                {
                    Id = reader.GetGuid(0),
                    IdCanco = reader.GetGuid(1),
                    Format = reader.GetString(2),
                    Bitrate = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Mida = reader.IsDBNull(4) ? null : reader.GetDecimal(4)
                };
            }

            dbConn.Close();
            return fitxer;
        }

        public static void Update(DatabaseConnection dbConn, QualitatFitxer qualitat)
        {
            dbConn.Open();

            string sql = @"UPDATE Files_quality
                           SET
                               bitrate = @bitrate,
                               format = @format
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", qualitat.Id);
            cmd.Parameters.AddWithValue("@bitrate", qualitat.Bitrate);
            cmd.Parameters.AddWithValue("@format", qualitat.Format);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Files_quality WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }

    }
}
