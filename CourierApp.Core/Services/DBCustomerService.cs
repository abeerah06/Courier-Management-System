using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CourierApp.Core.Services
{
    public class DBCustomerService(string conn) : ICustomerService
    {
        private readonly string _conn = conn;

        public ValidationResult Validate(Customer c)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(c.Name))
                result.AddError("Name is required.");
            else if (c.Name.Trim().Length < 2)
                result.AddError("Name must be at least 2 characters.");
            else if (c.Name.Trim().Length > 100)
                result.AddError("Name cannot exceed 100 characters.");
            else if (!Regex.IsMatch(c.Name.Trim(), @"^[a-zA-Z\s]+$"))
                result.AddError("Name can only contain letters and spaces.");

            if (string.IsNullOrWhiteSpace(c.Phone))
                result.AddError("Phone number is required.");
            else if (!Regex.IsMatch(c.Phone.Trim(), @"^\d{11}$"))
                result.AddError("Phone must be exactly 11 digits (e.g. 03001234567).");

            if (!string.IsNullOrWhiteSpace(c.Email))
            {
                if (!Regex.IsMatch(c.Email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    result.AddError("Email address is not valid.");
                else if (c.Email.Trim().Length > 100)
                    result.AddError("Email cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(c.Address))
                result.AddError("Address is required.");
            else if (c.Address.Trim().Length > 255)
                result.AddError("Address cannot exceed 255 characters.");

            if (string.IsNullOrWhiteSpace(c.City))
                result.AddError("City is required.");
            else if (c.City.Trim().Length > 100)
                result.AddError("City cannot exceed 100 characters.");

            return result;
        }

        public void Add(Customer c)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"INSERT INTO Customer (Name, Phone, Email, Address, City, CreatedAt)
                           VALUES (@Name, @Phone, @Email, @Address, @City, @CreatedAt)";
            SqlCommand cmd = new(sql, connection);
            BindParams(cmd, c);
            cmd.Parameters.AddWithValue("@CreatedAt", c.CreatedAt);
            cmd.ExecuteNonQuery();
        }

        public void Update(Customer c)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"UPDATE Customer SET Name=@Name, Phone=@Phone, Email=@Email,
                           Address=@Address, City=@City WHERE Id=@Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Id", c.Id);
            BindParams(cmd, c);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand del = new("DELETE FROM Customer WHERE Id=@Id", connection);
            del.Parameters.AddWithValue("@Id", id);
            del.ExecuteNonQuery();

            string reseed = @"IF (SELECT COUNT(*) FROM Customer) = 0
                                DBCC CHECKIDENT ('Customer', RESEED, 0);
                              ELSE BEGIN
                                DECLARE @m INT = ISNULL((SELECT MAX(Id) FROM Customer),0);
                                DBCC CHECKIDENT ('Customer', RESEED, @m);
                              END";
            new SqlCommand(reseed, connection).ExecuteNonQuery();
        }

        public Customer? GetById(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("SELECT * FROM Customer WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var r = cmd.ExecuteReader();
            return r.Read() ? MapRow(r) : null;
        }

        public List<Customer> GetAll()
        {
            var list = new List<Customer>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            var r = new SqlCommand("SELECT * FROM Customer ORDER BY Id", connection).ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var list = new List<Customer>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            SqlCommand cmd = new("SELECT * FROM Customer ORDER BY Id", connection);
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        public List<Customer> Search(string q)
        {
            var list = new List<Customer>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"SELECT * FROM Customer
                           WHERE Name LIKE @q OR Phone LIKE @q OR Email LIKE @q OR City LIKE @q
                           ORDER BY Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Customer>> SearchAsync(string q)
        {
            var list = new List<Customer>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            string sql = @"SELECT * FROM Customer
                           WHERE Name LIKE @q OR Phone LIKE @q OR Email LIKE @q OR City LIKE @q
                           ORDER BY Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        private static void BindParams(SqlCommand cmd, Customer c)
        {
            cmd.Parameters.AddWithValue("@Name", c.Name);
            cmd.Parameters.AddWithValue("@Phone", c.Phone);
            cmd.Parameters.AddWithValue("@Email", c.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", c.Address);
            cmd.Parameters.AddWithValue("@City", c.City);
        }

        private static Customer MapRow(SqlDataReader r) => new()
        {
            Id = Convert.ToInt32(r["Id"]),
            Name = r["Name"].ToString()!,
            Phone = r["Phone"].ToString()!,
            Email = r["Email"] == DBNull.Value ? null : r["Email"].ToString(),
            Address = r["Address"].ToString()!,
            City = r["City"].ToString()!,
            CreatedAt = Convert.ToDateTime(r["CreatedAt"])
        };
    }
}
