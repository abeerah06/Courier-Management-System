using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using Microsoft.Data.SqlClient;

namespace CourierApp.Core.Services
{
    public class DBPaymentService(string conn) : IPaymentService
    {
        private readonly string _conn = conn;

        public void Add(Payment p)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"INSERT INTO Payment (ShipmentId, CustomerId, CustomerName, TrackingNo, Amount, Status, PaymentDate)
                           VALUES (@ShipmentId, @CustomerId, @CustomerName, @TrackingNo, @Amount, @Status, @PaymentDate)";
            SqlCommand cmd = new(sql, connection);
            BindParams(cmd, p);
            cmd.ExecuteNonQuery();
        }

        public void Update(Payment p)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"UPDATE Payment SET ShipmentId=@ShipmentId, CustomerId=@CustomerId,
                           CustomerName=@CustomerName, TrackingNo=@TrackingNo,
                           Amount=@Amount, Status=@Status, PaymentDate=@PaymentDate
                           WHERE Id=@Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Id", p.Id);
            BindParams(cmd, p);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand del = new("DELETE FROM Payment WHERE Id=@Id", connection);
            del.Parameters.AddWithValue("@Id", id);
            del.ExecuteNonQuery();

            string reseed = @"IF (SELECT COUNT(*) FROM Payment) = 0
                                DBCC CHECKIDENT ('Payment', RESEED, 0);
                              ELSE BEGIN
                                DECLARE @m INT = ISNULL((SELECT MAX(Id) FROM Payment),0);
                                DBCC CHECKIDENT ('Payment', RESEED, @m);
                              END";
            new SqlCommand(reseed, connection).ExecuteNonQuery();
        }

        public void UpdateStatus(int id, string status)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("UPDATE Payment SET Status=@Status WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.ExecuteNonQuery();
        }

        public Payment? GetById(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("SELECT * FROM Payment WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var r = cmd.ExecuteReader();
            return r.Read() ? MapRow(r) : null;
        }

        public Payment? GetByShipmentId(int shipmentId)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("SELECT * FROM Payment WHERE ShipmentId=@ShipmentId", connection);
            cmd.Parameters.AddWithValue("@ShipmentId", shipmentId);
            var r = cmd.ExecuteReader();
            return r.Read() ? MapRow(r) : null;
        }

        public List<Payment> GetAll()
        {
            var list = new List<Payment>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            var r = new SqlCommand("SELECT * FROM Payment ORDER BY PaymentDate DESC", connection).ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public List<Payment> Search(string q)
        {
            var list = new List<Payment>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"SELECT * FROM Payment
                           WHERE CustomerName LIKE @q OR TrackingNo LIKE @q OR Status LIKE @q
                           ORDER BY PaymentDate DESC";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            var list = new List<Payment>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            SqlCommand cmd = new("SELECT * FROM Payment ORDER BY PaymentDate DESC", connection);
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Payment>> SearchAsync(string q)
        {
            var list = new List<Payment>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            string sql = @"SELECT * FROM Payment
                           WHERE CustomerName LIKE @q OR TrackingNo LIKE @q OR Status LIKE @q
                           ORDER BY PaymentDate DESC";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%");
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        private static void BindParams(SqlCommand cmd, Payment p)
        {
            cmd.Parameters.AddWithValue("@ShipmentId", p.ShipmentId);
            cmd.Parameters.AddWithValue("@CustomerId", p.CustomerId);
            cmd.Parameters.AddWithValue("@CustomerName", p.CustomerName);
            cmd.Parameters.AddWithValue("@TrackingNo", p.TrackingNo);
            cmd.Parameters.AddWithValue("@Amount", p.Amount);
            cmd.Parameters.AddWithValue("@Status", p.Status);
            cmd.Parameters.AddWithValue("@PaymentDate", p.PaymentDate);
        }

        private static Payment MapRow(SqlDataReader r) => new()
        {
            Id = Convert.ToInt32(r["Id"]),
            ShipmentId = Convert.ToInt32(r["ShipmentId"]),
            CustomerId = Convert.ToInt32(r["CustomerId"]),
            CustomerName = r["CustomerName"].ToString()!,
            TrackingNo = r["TrackingNo"].ToString()!,
            Amount = Convert.ToDecimal(r["Amount"]),
            Status = r["Status"].ToString()!,
            PaymentDate = Convert.ToDateTime(r["PaymentDate"])
        };
    }
}
