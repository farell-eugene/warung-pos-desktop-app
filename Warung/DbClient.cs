using System;
using System.Data;
using System.Data.SqlClient;

namespace Warung
{
    public class DbClient
    {
        private readonly string connectionString;

        public DbClient()
        {
            connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=warung;Integrated Security=True;";
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable ExecuteSelect(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ExecuteSelect(string query)
        {
            return ExecuteSelect(query, null);
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }

        public bool LoginUser(string email, string password)
        {
            string query = "SELECT COUNT(*) FROM pemilik_warung WHERE email = @email AND password = @password";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)
            };

            object result = ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }
    }
}
