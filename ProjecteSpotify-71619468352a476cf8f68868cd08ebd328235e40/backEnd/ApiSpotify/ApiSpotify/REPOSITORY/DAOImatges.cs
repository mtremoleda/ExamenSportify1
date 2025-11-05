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
    public class DAOImatges
    {
        public static void Insert(DatabaseConnection dbConn, Imatge imatge)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Perfils (Id, Titul, Descripcio)
                          VALUES (@Id, @Nom, @Descripcio)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", imatge.Id);
            cmd.Parameters.AddWithValue("@Titul", imatge.Titul);
            cmd.Parameters.AddWithValue("@Descripcio", imatge.Descripcio);
            


            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");
            dbConn.Close();
        }

        public static List<Imatge> GetAll(DatabaseConnection dbconn)
        {
            List<Imatge> imatge = new();

            dbconn.Open();

            string sql = "SELECT Id, Titul, Descripcio FROM Imatge";

            using SqlCommand cmd = new SqlCommand(sql, dbconn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                imatge.Add(new Imatge
                {
                    Id = reader.GetGuid(0),
                    Titul = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                   

                });
            }

            dbconn.Close();
            return imatge;
        }

        public static Imatge GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();
            string sql = "SELECT Id, Titul, Descripcio FROM Imatges";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            Imatge? imatge= null;

            if (reader.Read())
            {
                imatge = new Imatge
                {
                    Id = reader.GetGuid(0),
                    Titul = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                   

                };
            }
            ;

            dbConn.Close();
            return imatge;
        }

        public static void Update(DatabaseConnection dbConn, Imatge imatge)
        {
            dbConn.Open();

            string sql = @"UPDATE Imatge
                           SET Titul = @titul,
                               Descripcio = @descripcio
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", imatge.Id);
            cmd.Parameters.AddWithValue("@titul", imatge.Titul);
            cmd.Parameters.AddWithValue("@descripcio", imatge.Descripcio);
            


            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Imatges WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }
    }
}
