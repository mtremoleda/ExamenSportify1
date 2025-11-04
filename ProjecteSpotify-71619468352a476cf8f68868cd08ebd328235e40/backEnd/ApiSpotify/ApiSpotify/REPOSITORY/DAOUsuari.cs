using ApiSpotify.MODELS; // Canvia-ho pel teu namespace real
using ApiSpotify.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ApiSpotify.REPOSITORY
{
    public class DAOUsuari
    {
        public static void Insert(DatabaseConnection dbConn, Usuari usuari, string salt)
        {

            dbConn.Open();

            string sql = @"INSERT INTO Users (Id, nom, contrasenya, salt)
                           VALUES (@Id, @nom, @contrasenya, @salt)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);

            cmd.Parameters.AddWithValue("@Id", usuari.Id);
            cmd.Parameters.AddWithValue("@nom", usuari.Nom);
            cmd.Parameters.AddWithValue("@contrasenya", usuari.Contrasenya);
            cmd.Parameters.AddWithValue("@salt", salt);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");

            dbConn.Close();
        }

        public static List<Usuari> GetAll(DatabaseConnection dbConn)
        {
            List<Usuari> usuaris = new();
            dbConn.Open();

            string sql = "SELECT Id, nom, contrasenya, salt FROM Users";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                usuaris.Add(new Usuari
                {
                    Id = reader.GetGuid(0),
                    Nom = reader.GetString(1),
                    Contrasenya = reader.GetString(2),
                    Salt = reader.GetString(3)
                });
            }

            dbConn.Close();
            return usuaris;
        }

        public static Usuari? GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = "SELECT Id, nom, contrasenya, salt FROM Users WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            Usuari? usuari = null;

            if (reader.Read())
            {
                usuari = new Usuari
                {
                    Id = reader.GetGuid(0),
                    Nom = reader.GetString(1),
                    Contrasenya = reader.GetString(2),
                    Salt = reader.GetString(3)
                };
            }

            dbConn.Close();
            return usuari;
        }

        public static void Update(DatabaseConnection dbConn, Usuari usuari)
        {

            string salt = UTILS.UtilsContrasenya.GenerateSalt();
            string hashedPassword = UTILS.UtilsContrasenya.HashPassword(usuari.Contrasenya, salt);


            dbConn.Open();

            string sql = @"UPDATE Users
                           SET Nom = @Nom,
                               Contrasenya = @Contrasenya,
                               Salt = @Salt
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", usuari.Id);
            cmd.Parameters.AddWithValue("@Nom", usuari.Nom);
            cmd.Parameters.AddWithValue("@contrasenya", hashedPassword);
            cmd.Parameters.AddWithValue("@salt", salt);


            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Users WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }
    }
}