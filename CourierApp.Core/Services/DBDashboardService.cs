using CourierApp.Core.Contracts;
using Microsoft.Data.SqlClient;

namespace CourierApp.Core.Services
{
    public class DBDashboardService(string conn) : IDashboardService
    {
        private readonly string _conn = conn;

        public int GetTotalShipments()
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            return (int)new SqlCommand("SELECT COUNT(*) FROM Shipment", connection).ExecuteScalar()!;
        }

        public int GetShipmentsThisMonth()
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"SELECT COUNT(*) FROM Shipment
                           WHERE MONTH(CreatedAt)=MONTH(GETDATE()) AND YEAR(CreatedAt)=YEAR(GETDATE())";
            return (int)new SqlCommand(sql, connection).ExecuteScalar()!;
        }

        public int GetShipmentsThisWeek()
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Shipment WHERE CreatedAt >= DATEADD(DAY,-7,GETDATE())";
            return (int)new SqlCommand(sql, connection).ExecuteScalar()!;
        }

        public int GetShipmentsToday()
        {
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = "SELECT COUNT(*) FROM Shipment WHERE CAST(CreatedAt AS DATE)=CAST(GETDATE() AS DATE)";
            return (int)new SqlCommand(sql, connection).ExecuteScalar()!;
        }

        public Dictionary<string, int> GetMonthlyShipments(int months)
        {
            var result = new Dictionary<string, int>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = @"SELECT FORMAT(CreatedAt,'MMM yyyy') AS Month, COUNT(*) AS Total
                           FROM Shipment
                           WHERE CreatedAt >= DATEADD(MONTH,@months,GETDATE())
                           GROUP BY FORMAT(CreatedAt,'MMM yyyy'), YEAR(CreatedAt), MONTH(CreatedAt)
                           ORDER BY YEAR(CreatedAt), MONTH(CreatedAt)";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@months", -months);
            var r = cmd.ExecuteReader();
            while (r.Read())
                result[r["Month"].ToString()!] = Convert.ToInt32(r["Total"]);
            return result;
        }

        public Dictionary<string, int> GetStatusBreakdown()
        {
            var result = new Dictionary<string, int>();
            using SqlConnection connection = new(_conn);
            connection.Open();
            string sql = "SELECT Status, COUNT(*) AS Total FROM Shipment GROUP BY Status";
            var r = new SqlCommand(sql, connection).ExecuteReader();
            while (r.Read())
                result[r["Status"].ToString()!] = Convert.ToInt32(r["Total"]);
            return result;
        }

        public async Task<int> GetTotalShipmentsAsync()
        {
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            SqlCommand cmd = new("SELECT COUNT(*) FROM Shipment", connection);
            return (int)(await cmd.ExecuteScalarAsync())!;
        }

        public async Task<Dictionary<string, int>> GetMonthlyShipmentsAsync(int months)
        {
            var result = new Dictionary<string, int>();
            using SqlConnection connection = new(_conn);
            await connection.OpenAsync();
            string sql = @"SELECT FORMAT(CreatedAt,'MMM yyyy') AS Month, COUNT(*) AS Total
                           FROM Shipment
                           WHERE CreatedAt >= DATEADD(MONTH,@months,GETDATE())
                           GROUP BY FORMAT(CreatedAt,'MMM yyyy'), YEAR(CreatedAt), MONTH(CreatedAt)
                           ORDER BY YEAR(CreatedAt), MONTH(CreatedAt)";
            SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@months", -months);
            var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
                result[r["Month"].ToString()!] = Convert.ToInt32(r["Total"]);
            return result;
        }
    }
}
