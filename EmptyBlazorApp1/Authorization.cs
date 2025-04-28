using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using Npgsql;

namespace EmptyBlazorApp1
{
    public class Authorization : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private bool _disposed = false;

        public Authorization(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool validateUser(string login, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))    
                {
                   //MessageBox.Show("Введите логин и пароль");
                    return false;
                }
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    bool passwordCorrect = VerifyPassword(connection, login, password);
                    if (!passwordCorrect)
                    {
                        //MessageBox.Show("Неверный пароль");
                        return false;
                    }
                    return true;
                }

            }
            catch
            {
                
                return false;
            }
        }

        private bool CheckUserExists(NpgsqlConnection connection, string login)
        {
            const string query = @"SELECT 1 FROM ""Пользователи"" WHERE ""Name"" = @username LIMIT 1";
            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", login);
                return cmd.ExecuteScalar() != null;
            }
        }

        public bool GetUserExist(NpgsqlConnection connection, string login)
        {
            return CheckUserExists(connection, login);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection?.Dispose();
                _disposed = true;
            }
        }

        private bool VerifyPassword(NpgsqlConnection connection, string login, string inputPassword)
        {
            const string query = @"SELECT ""password"" FROM ""Пользователи"" WHERE ""Name"" = @username";
            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", login);
                var storedHash = cmd.ExecuteScalar()?.ToString();

                if (string.IsNullOrEmpty(storedHash))
                    return false;

                return PasswordHasher.VerifyPassword(inputPassword, storedHash);
            }
        }
    }
}