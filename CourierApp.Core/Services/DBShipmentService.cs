using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using Microsoft.Data.SqlClient;

namespace CourierApp.Core.Services
{
    public class DBShipmentService(string conn) : IShipmentService
    {
        private readonly string _conn = conn;

        public ValidationResult Validate(Shipment s)
        {
            var result = new ValidationResult();

            if (s.CustomerId <= 0)
                result.AddError("Please select a customer.");

            if (string.IsNullOrWhiteSpace(s.Origin))
                result.AddError("Origin city is required.");
            else if (s.Origin.Trim().Length > 100)
                result.AddError("Origin cannot exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(s.Destination))
                result.AddError("Destination city is required.");
            else if (s.Destination.Trim().Length > 100)
                result.AddError("Destination cannot exceed 100 characters.");

            if (s.WeightKg <= 0)
                result.AddError("Weight must be greater than 0 kg.");
            else if (s.WeightKg > 1000)
                result.AddError("Weight cannot exceed 1000 kg.");

            if (s.Cost <= 0)
                result.AddError("Cost must be greater than 0.");

            if (s.PickupDate == default)
                result.AddError("Pickup date is required.");

            if (!string.IsNullOrWhiteSpace(s.Notes) && s.Notes.Trim().Length > 500)
                result.AddError("Notes cannot exceed 500 characters.");

            return result;
        }

        public void Add(Shipment s)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"INSERT INTO Shipment
                           (CustomerId, CustomerName, DriverId, DriverName, TrackingNo, Origin, Destination,
                            WeightKg, Cost, Status, PickupDate, DeliveryDate, Notes, CreatedAt)
                           VALUES
                           (@CustomerId, @CustomerName, @DriverId, @DriverName, @TrackingNo, @Origin, @Destination,
                            @WeightKg, @Cost, @Status, @PickupDate, @DeliveryDate, @Notes, @CreatedAt)";
            SqlCommand cmd = new(sql, connection);
            BindParams(cmd, s);
            cmd.ExecuteNonQuery();
        }

        public void Update(Shipment s)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"UPDATE Shipment SET CustomerId=@CustomerId, CustomerName=@CustomerName,
                           DriverId=@DriverId, DriverName=@DriverName,
                           TrackingNo=@TrackingNo, Origin=@Origin, Destination=@Destination,
                           WeightKg=@WeightKg, Cost=@Cost, Status=@Status,
                           PickupDate=@PickupDate, DeliveryDate=@DeliveryDate, Notes=@Notes
                           WHERE Id=@Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Id", s.Id);
            BindParams(cmd, s);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand del = new("DELETE FROM Shipment WHERE Id=@Id", connection);
            del.Parameters.AddWithValue("@Id", id);
            del.ExecuteNonQuery();

            string reseed = @"IF (SELECT COUNT(*) FROM Shipment) = 0
                                DBCC CHECKIDENT ('Shipment', RESEED, 0);
                              ELSE BEGIN
                                DECLARE @m INT = ISNULL((SELECT MAX(Id) FROM Shipment),0);
                                DBCC CHECKIDENT ('Shipment', RESEED, @m);
                              END";
            new SqlCommand(reseed, connection).ExecuteNonQuery();
        }

        public void UpdateStatus(int id, string status)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = status == "Delivered"
                ? "UPDATE Shipment SET Status=@Status, DeliveryDate=GETDATE() WHERE Id=@Id"
                : "UPDATE Shipment SET Status=@Status WHERE Id=@Id";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.ExecuteNonQuery();
        }

        public Shipment? GetById(int id)
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            SqlCommand cmd = new("SELECT * FROM Shipment WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var r = cmd.ExecuteReader();
            return r.Read() ? MapRow(r) : null;
        }

        public List<Shipment> GetAll()
        {
            var list = new List<Shipment>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            var r = new SqlCommand("SELECT * FROM Shipment ORDER BY CreatedAt DESC", connection).ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Shipment>> GetAllAsync()
        {
            var list = new List<Shipment>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            SqlCommand cmd = new("SELECT * FROM Shipment ORDER BY CreatedAt DESC", connection);
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        public List<Shipment> Search(string query, string statusFilter = "All")
        {
            var list = new List<Shipment>();
            using SqlConnection connection = new(_conn);
            connection.Open();

            string statusClause = statusFilter == "All" ? "" : " AND Status=@Status";
            string sql = $@"SELECT * FROM Shipment
                            WHERE (CustomerName LIKE @q OR TrackingNo LIKE @q
                                   OR Origin LIKE @q OR Destination LIKE @q)
                            {statusClause}
                            ORDER BY CreatedAt DESC";

            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + query + "%");
            if (statusFilter != "All")
                cmd.Parameters.AddWithValue("@Status", statusFilter);

            var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(MapRow(r));
            return list;
        }

        public async Task<List<Shipment>> SearchAsync(string query, string statusFilter = "All")
        {
            var list = new List<Shipment>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();

            string statusClause = statusFilter == "All" ? "" : " AND Status=@Status";
            string sql = $@"SELECT * FROM Shipment
                            WHERE (CustomerName LIKE @q OR TrackingNo LIKE @q
                                   OR Origin LIKE @q OR Destination LIKE @q)
                            {statusClause}
                            ORDER BY CreatedAt DESC";

            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@q", "%" + query + "%");
            if (statusFilter != "All")
                cmd.Parameters.AddWithValue("@Status", statusFilter);

            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync()) list.Add(MapRow(r));
            return list;
        }

        public string GenerateTrackingNo()
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            int count = (int)new SqlCommand("SELECT COUNT(*) FROM Shipment", connection).ExecuteScalar()!;
            return $"CR-{DateTime.Now:yyyy}-{(count + 1):D4}";
        }

        private static void BindParams(SqlCommand cmd, Shipment s)
        {
            cmd.Parameters.AddWithValue("@CustomerId", s.CustomerId);
            cmd.Parameters.AddWithValue("@CustomerName", s.CustomerName);
            cmd.Parameters.AddWithValue("@DriverId", s.DriverId.HasValue ? (object)s.DriverId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@DriverName", s.DriverName);
            cmd.Parameters.AddWithValue("@TrackingNo", s.TrackingNo);
            cmd.Parameters.AddWithValue("@Origin", s.Origin);
            cmd.Parameters.AddWithValue("@Destination", s.Destination);
            cmd.Parameters.AddWithValue("@WeightKg", s.WeightKg);
            cmd.Parameters.AddWithValue("@Cost", s.Cost);
            cmd.Parameters.AddWithValue("@Status", s.Status);
            cmd.Parameters.AddWithValue("@PickupDate", s.PickupDate);
            cmd.Parameters.AddWithValue("@DeliveryDate", s.DeliveryDate.HasValue ? (object)s.DeliveryDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", s.Notes ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", s.CreatedAt);
        }

        private static Shipment MapRow(SqlDataReader r) => new()
        {
            Id = Convert.ToInt32(r["Id"]),
            CustomerId = Convert.ToInt32(r["CustomerId"]),
            CustomerName = r["CustomerName"].ToString()!,
            DriverId = r["DriverId"] == DBNull.Value ? null : Convert.ToInt32(r["DriverId"]),
            DriverName = r["DriverName"] == DBNull.Value ? "Unassigned" : r["DriverName"].ToString()!,
            TrackingNo = r["TrackingNo"].ToString()!,
            Origin = r["Origin"].ToString()!,
            Destination = r["Destination"].ToString()!,
            WeightKg = Convert.ToDecimal(r["WeightKg"]),
            Cost = Convert.ToDecimal(r["Cost"]),
            Status = r["Status"].ToString()!,
            PickupDate = Convert.ToDateTime(r["PickupDate"]),
            DeliveryDate = r["DeliveryDate"] == DBNull.Value ? null : Convert.ToDateTime(r["DeliveryDate"]),
            Notes = r["Notes"] == DBNull.Value ? null : r["Notes"].ToString(),
            CreatedAt = Convert.ToDateTime(r["CreatedAt"])
        };
    }
}
