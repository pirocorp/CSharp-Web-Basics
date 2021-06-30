namespace SharedTrip.Services
{
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            using var sha = SHA256.Create();

            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            var builder = new StringBuilder();

            foreach (var @byte in bytes)
            {
                builder.Append(@byte.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
