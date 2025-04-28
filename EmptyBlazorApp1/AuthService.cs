using System.Threading.Tasks;
namespace EmptyBlazorApp1
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public class AuthResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }

        }

        public async Task<AuthResult> LoginAsync(string login, string password)
        {
            using var auth = new Authorization(_connectionString);
            bool isValid = auth.validateUser(login, password);

            return new AuthResult
            {
                Success = isValid,
                Message = isValid ? "Успешный вход" : "Неверный логин или пароль"
            };
        }

        public async Task<AuthResult> RegisterAsync(string login, string password)
        {
            try
            {
                using var reg = new Registration(_connectionString);
                return reg.RegisterUser(login, password);
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = $"Ошибка: {ex.Message}"
                };
            }
        }
    }
}
