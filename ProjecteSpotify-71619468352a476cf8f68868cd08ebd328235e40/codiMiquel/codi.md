EndPoints/Product.cs

using dbdemo.Repository;
using dbdemo.Services;
using dbdemo.Model;

namespace dbdemo.Endpoints;

public static class EndpointsProducts
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /products
        app.MapGet("/products", () =>
        {
            List<Product>  products = ProductADO.GetAll(dbConn);
            return Results.Ok(products);
        });

        // GET Product by id
        app.MapGet("/products/{id}", (Guid id) =>
        {
            Product? product = ProductADO.GetById(dbConn, id);

            return product is not null
                ? Results.Ok(product)
                : Results.NotFound(new { message = $"Product with Id {id} not found." });
        });

        // POST /products
        app.MapPost("/products", (ProductRequest req) =>
        {
            Product product = new Product
            {
                Id = Guid.NewGuid(),
                Code = req.Code,
                Name = req.Name,
                Price = req.Price
            };

            ProductADO.Insert(dbConn, product);

            return Results.Created($"/products/{product.Id}", product);
        });

        app.MapPut("/products/{id}", (Guid id, ProductRequest req) =>
        {
            var existing = ProductADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Product updated = new Product
            {
                Id = id,
                Code = req.Code,
                Name = req.Name,
                Price = req.Price
            };

            ProductADO.Update(dbConn, updated);

            return Results.Ok(updated);
        });

        // DELETE /products/{id}
        app.MapDelete("/products/{id}", (Guid id) => ProductADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        // POST  /products/{id}/upload

        app.MapPost("/products/{id}/upload", async (Guid id, IFormFile image) =>
        {
            if (image == null || image.Length == 0)
                return Results.BadRequest(new { message = "No s'ha rebut cap imatge." });

           
            Product? product = ProductADO.GetById(dbConn, id);
            if (product is null)
                return Results.NotFound(new { message = $"Producte amb Id {id} no trobat." });

            string filePath = await SaveImage(id,image);            

            product.ImagePath = filePath;
            ProductADO.Update(dbConn, product);

            return Results.Ok(new { message = "Imatge pujada correctament.", path = filePath });
        }).DisableAntiforgery();
    }

    public static async Task<string> SaveImage(Guid id, IFormFile image)
    {
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = $"{id}_{Path.GetFileName(image.FileName)}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return filePath;
    }
}

// DTO pel request
public record ProductRequest(string Code, string Name, decimal Price);

____________________________________________________________________________

Repository/ProductADO.cs

using Microsoft.Data.SqlClient;
using static System.Console;
using dbdemo.Services;
using dbdemo.Model;

namespace dbdemo.Repository;

class ProductADO
{
   
    public static void Insert(DatabaseConnection dbConn,Product product)    // El mètode ha de passar a ser static
    {

        dbConn.Open();

        string sql = @"INSERT INTO Products (Id, Code, Name, Price)
                        VALUES (@Id, @Code, @Name, @Price)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Code", product.Code);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");
        dbConn.Close();
    }

    public static void Update(DatabaseConnection dbConn, Product product)
    {
        dbConn.Open();

        string sql = @"UPDATE Products
                    SET Code = @Code,
                        Name = @Name,
                        Price = @Price,
                        Image = @Image
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Code", product.Code);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Image", product.ImagePath);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} fila actualitzada.");
       
        dbConn.Close();
    }

    public static List<Product> GetAll(DatabaseConnection dbConn)
    {
        List<Product> products = new();

        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
        }

        dbConn.Close();
        return products;
    }

    public static Product? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Product? product = null;    // Si no inicialitzem la variable => no existeix en el return!

        if (reader.Read())
        {
            product = new Product
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }

        dbConn.Close();
        return product;
    }

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}

Model/Product.cs

namespace dbdemo.Model;

public class Product
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string ImagePath { get; set; } = "";
}