using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CourierApp.Core.Services
{
    public class DBDriverService(string conn) : IDriverService
    {
        private readonly string _conn = conn;

        public ValidationResult Validate(Driver d)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(d.Name))
                result.AddError("Name is required.");
            else if (d.Name.Trim().Length < 2)
                result.AddError("Name must be at least 2 characters.");
            else if (d.Name.Trim().Length > 100)
                result.AddError("Name cannot exceed 100 characters.");
            else if (!Regex.IsMatch(d.Name.Trim(), @"^[a-zA-Z\s]+$"))
                result.AddError("Name can only contain letters and spaces.");

            if (string.IsNullOrWhiteSpace(d.Phone))
                result.AddError("Phone number is required.");
            else if (!Regex.IsMatch(d.Phone.Trim(), @"^\d{11}$"))
                result.AddError("Phone must be exactly 11 digits (e.g. 03001234567).");

            if (string.IsNullOrWhiteSpace(d.VehicleType))
                result.AddError("Vehicle type is required.");

            if (string.IsNullOrWhiteSpace(d.VehicleNo))
                result.AddError("Vehicle number is required.");
            else if (d.VehicleNo.Trim().Length > 20)
                result.AddError("Vehicle number cannot exceed 20 characters.");

            if (string.IsNullOrWhiteSpace(d.City))
                result.AddError("City is required.");
            else if (d.City.Trim().Length > 100)
                result.AddError("City cannot exceed 100 characters.");

            return result;
        }

        public void Add(Driver d)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"INSERT INTO Driver (Name, Phone, VehicleType, VehicleNo, City, IsActive, CreatedAt)
                           VALUES (@Name, @Phone, @VehicleType, @VehicleNo, @City, @IsActive, @CreatedAt)";
            SqlCommand cmd = new(sql, connection);
            BindParams(cmd, d);
            cmd.Parameters.AddWithValue("@CreatedAt", d.CreatedAt);
            cmd.ExecuteNonQuery();
        }

        public void Update(Driver d)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"UPDATE Driver SET Name=@Name, Phone=@Phone, VehicleType=@VehicleType,
                           VehicleNo=@VehicleNo, City=@City, IsActive=@IsActive WHERE Id=@Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Id", d.Id);
            BindParams(cmd, d);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand del = new("DELETE FROM Driver WHERE Id=@Id", connection);
            del.Parameters.AddWithValue("@Id", id);
            del.ExecuteNonQuery();

            string reseed = @"IF (SELECT COUNT(*) FROM Driver) = 0
                                DBCC CHECKIDENT ('Driver', RESEED, 0);
                              ELSE BEGIN
                                DECLARE @m INT = ISNULL((SELECT MAX(Id) FROM Driver),0);
                                DBCC CHECKIDENT ('Driver', RESEED, @m);
                              END";
            new SqlCommand(reseed, connection).ExecuteNonQuery();
        }

        public Driver? GetById(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("SELECT * FROM Driver WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var r = cmd.ExecuteReader();
            return r.Read() ? MapRow(r) : null;
        }

        public List<Driver> GetAll()
        {
            var list = new List<Driver>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            var r = new SqlCommand("SELECT * FROM Driver ORDER BY Id", connection).ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Driver>> GetAllAsync()
        {
            var list = new List<Driver>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            SqlCommand cmd = new("SELECT * FROM Driver ORDER BY Id", connection);
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        public List<Driver> GetActive()
        {
            var list = new List<Driver>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            var r = new SqlCommand("SELECT * FROM Driver WHERE IsActive=1 ORDER BY Name", connection).ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public List<Driver> Search(string q)
        {
            var list = new List<Driver>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"SELECT * FROM Driver
                           WHERE Name LIKE @q OR Phone LIKE @q OR VehicleNo LIKE @q OR City LIKE @q
                           ORDER BY Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Driver>> SearchAsync(string q)
        {
            var list = new List<Driver>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            string sql = @"SELECT * FROM Driver
                           WHERE Name LIKE @q OR Phone LIKE @q OR VehicleNo LIKE @q OR City LIKE @q
                           ORDER BY Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        private static void BindParams(SqlCommand cmd, Driver d)
        {
            cmd.Parameters.AddWithValue("@Name", d.Name);
            cmd.Parameters.AddWithValue("@Phone", d.Phone);
            cmd.Parameters.AddWithValue("@VehicleType", d.VehicleType);
            cmd.Parameters.AddWithValue("@VehicleNo", d.VehicleNo);
            cmd.Parameters.AddWithValue("@City", d.City);
            cmd.Parameters.AddWithValue("@IsActive", d.IsActive);
        }

        private static Driver MapRow(SqlDataReader r) => new()
        {
            Id = Convert.ToInt32(r["Id"]),
            Name = r["Name"].ToString()!,
            Phone = r["Phone"].ToString()!,
            VehicleType = r["VehicleType"].ToString()!,
            VehicleNo = r["VehicleNo"].ToString()!,
            City = r["City"].ToString()!,
            IsActive = Convert.ToBoolean(r["IsActive"]),
            CreatedAt = Convert.ToDateTime(r["CreatedAt"])
        };
    }
}
