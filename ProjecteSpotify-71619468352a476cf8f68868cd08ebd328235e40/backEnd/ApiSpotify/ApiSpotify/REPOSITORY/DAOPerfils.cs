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
    public class DAOPerfils
    {
        public static void Insert(DatabaseConnection dbConn, Perfil perfil)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Perfils (Id, Nom, Descripcio, Estat)
                          VALUES (@Id, @Nom, @Descripcio, @Estat)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", perfil.Id);
            cmd.Parameters.AddWithValue("@Nom", perfil.Nom);
            cmd.Parameters.AddWithValue("@Descripcio", perfil.Descripcio);
            cmd.Parameters.AddWithValue("@Estat", perfil.Estat);
            

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");
            dbConn.Close();
        }

        public static List<Perfil> GetAll(DatabaseConnection dbconn)
        {
            List<Perfil> perfil = new();

            dbconn.Open();

            string sql = "SELECT Id, Nom, Descripcio, Estat FROM Perfils";

            using SqlCommand cmd = new SqlCommand(sql, dbconn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                perfil.Add(new Perfil
                {
                    Id = reader.GetGuid(0),
                    Nom = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                    Estat = reader.GetString(3),
                    
                });
            }

            dbconn.Close();
            return perfil;
        }

        public static Perfil GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();
            string sql = "SELECT Id, Nom, Descripcio, Estat FROM Perfils";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            Perfil? perfil = null;

            if (reader.Read())
            {
                perfil = new Perfil
                {
                    Id = reader.GetGuid(0),
                    Nom = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                    Estat = reader.GetString(3),
                    
                };
            }
            ;

            dbConn.Close();
            return perfil;
        }

        public static void Update(DatabaseConnection dbConn, Perfil perfil)
        {
            dbConn.Open();

            string sql = @"UPDATE Perfils
                           SET Nom = @nom,
                               Descripcio = @descripcio,
                               Estat = @estat
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", perfil.Id);
            cmd.Parameters.AddWithValue("@nom", perfil.Nom);
            cmd.Parameters.AddWithValue("@descripcio", perfil.Descripcio);
            cmd.Parameters.AddWithValue("@estat", perfil.Estat);
            

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Perfils WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }
    }
}
