using Npgsql;
using static EmptyBlazorApp1.AuthService;

namespace EmptyBlazorApp1
{
    public class Registration : IDisposable
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private bool _disposed = false;

        public Registration(string connectionString)
        {
            _connectionString = connectionString;
        }

        private AuthResult TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    return new AuthResult
                    {
                        Success = true,
                        Message = "Подключение к БД успешно"
                    };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = $"Ошибка подключения к БД"
                };
            }
        }

        public AuthResult RegisterUser(string login, string password)
        {
            try
            {
                

                var connectionTest = TestConnection();
                if (!connectionTest.Success)
                {
                    return new AuthResult
                    {
                        Success = false,
                        Message = "Ошибка подключения к БД"
                    };
                }

                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Проверяем существование пользователя в одной сессии
                    if (UserExists(connection, login))
                    {
                        return new AuthResult
                        {
                            Success = false,
                            Message = "Пользователь существует"
                        };
                    }

                    string passwordHash = PasswordHasher.HashPassword(password);

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = @"INSERT INTO ""Пользователи"" (""Name"", ""password"") VALUES(@login, @password)";
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", passwordHash);
                        cmd.ExecuteNonQuery();
                        return new AuthResult
                        {
                            Success = true,
                            Message = "Успех!"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return new AuthResult
                {
                    Success = false,
                    Message = "Ошибка"
                };
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection?.Dispose();
                _disposed = true;
            }
        }

        private bool UserExists(NpgsqlConnection connection, string login)
        {
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT 1 FROM ""Пользователи"" WHERE ""Name"" = @login LIMIT 1";
                cmd.Parameters.AddWithValue("@login", login);
                return cmd.ExecuteScalar() != null;
            }
        }

    }

}