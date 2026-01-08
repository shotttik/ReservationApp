using System.Security.Cryptography;
using System.Text;

namespace Shared.Utilities
{
    public class CodeHasher
    {
        // Method to compute the SHA256 hash of an input string and return it as a hex string
        public static string ComputeSha256Hash(string rawData)
        {
            // Use SHA256.HashData to compute the hash (available in .NET 5+)
            // This avoids instantiating and disposing a new instance each time
            byte [] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));

            // Convert the byte array to a hexadecimal string
            return Convert.ToHexString(bytes);
        }

        // Method to compare two input strings by their SHA256 hashes
        public static bool CompareCode(string code1, string code2)
        {
            // Compute hashes for both code snippets
            string hash1 = ComputeSha256Hash(code1);
            string hash2 = ComputeSha256Hash(code2);

            // Compare the resulting hash strings (case-insensitive for hex strings)
            return StringComparer.OrdinalIgnoreCase.Compare(hash1, hash2) == 0;
        }
        // Method To compare two hashes
        public static bool CompareHashes(string hash1, string hash2)
            => StringComparer.OrdinalIgnoreCase.Compare(hash1, hash2) == 0;
        // Method To compare one code and one hash
        public static bool CompareCodeAndHash(string code1, string hash2)
        {
            string hash1 = ComputeSha256Hash(code1);

            // Compare the resulting hash strings (case-insensitive for hex strings)
            return StringComparer.OrdinalIgnoreCase.Compare(hash1, hash2) == 0;
        }
        // Method To generate hash code
        public static string GenerateAndHash(int length, out string code)
        {
            var random = new Random();
            int min = (int)Math.Pow(10, length - 1);
            int max = (int)Math.Pow(10, length);
            code = random.Next(min, max).ToString();
            var hash = ComputeSha256Hash(code);

            return hash;
        }
    }
}
