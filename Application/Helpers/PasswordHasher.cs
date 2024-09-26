using System.Security.Cryptography;
namespace Application.Helpers
{

    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit

        public static (byte [] hash, byte [] salt) HashPassword(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                10000,
                HashAlgorithmName.SHA256);
            var key = algorithm.GetBytes(KeySize);
            var salt = algorithm.Salt;

            return (hash: key, salt);
        }

        public static bool VerifyPassword(string password, byte [] hash, byte [] salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                10000,
                HashAlgorithmName.SHA256);
            var key = algorithm.GetBytes(KeySize);

            return key.SequenceEqual(hash);
        }
    }
}
