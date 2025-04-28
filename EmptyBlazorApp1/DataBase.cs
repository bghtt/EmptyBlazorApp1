using Npgsql;
using System.Data;

namespace EmptyBlazorApp1
{
    public class DataBase
    {
        private readonly string _connectionString = "server=localhost;Port=5432;Database=database;User Id = postgres; Password=rootroot";
        private NpgsqlConnection? _connection;
        public required NpgsqlDataAdapter pgDataAdapter;

        public bool Connect()
        {
            try
            {
                _connection = new NpgsqlConnection(_connectionString);
                _connection.Open();
                return true;
            }
            catch
            {
                //MessageBox.Show("Ошибка соединения с бд", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void closeConnection()
        {
            _connection?.Close();
        }

        public NpgsqlConnection? GetConnection()
        {
            return _connection;
        }

        public NpgsqlCommand CreateCommand(string query)
        {
            if (_connection?.State != ConnectionState.Open)
                throw new InvalidOperationException("Соединение не открыто");

            return new NpgsqlCommand(query, _connection);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
