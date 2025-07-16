using Contract.Interfaces.Product;
using Contract.Models;
using Dapper;
using System.Data.SqlClient;
using Utils;

namespace DataAccess
{
    public class ProductDADapper : IProductDA
    {
        private readonly string _connectionString = Utilities.GetEnvironmentVariable("SqlServerConnectionString");

        public async Task Create(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("INSERT INTO Products (Name, Price, Description) VALUES (@Name, @Price, @Description)", product);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<Product?> Get(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<List<Product>> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return (await connection.QueryAsync<Product>("SELECT * FROM Products")).ToList();
        }

        public async Task Update(int id, Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("UPDATE Products SET Name = @Name, Price = @Price, Description = @Description WHERE Id = @Id", product);
        }
    }
}
