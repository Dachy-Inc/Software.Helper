using System;
using System.Security.Cryptography;
using System.Text;

namespace Software.Helper
{
    public static class PasswordManager
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000; // Adjust for security/performance trade-off

        /// <summary>
        /// Hashes a password using PBKDF2 with a randomly generated salt.
        /// The format of the stored value is: {iterations}.{salt}.{hash}
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>A base64-encoded string containing the salt, iterations, and hash.</returns>
        public static string HashPassword(string password)
        {
            // Generate a random salt
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            // Derive a 256-bit subkey (hash) using PBKDF2
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            // Store the format: {iterations}.{salt}.{hash} as base64 parts
            string iterations = Iterations.ToString();
            string saltBase64 = Convert.ToBase64String(salt);
            string keyBase64 = Convert.ToBase64String(key);

            return $"{iterations}.{saltBase64}.{keyBase64}";
        }

        /// <summary>
        /// Verifies a provided password against the stored hashed password.
        /// </summary>
        /// <param name="hashedPassword">The stored hashed password string.</param>
        /// <param name="providedPassword">The plaintext password to verify.</param>
        /// <returns>True if they match, otherwise false.</returns>
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 3)
            {
                return false; // Invalid hash format
            }

            if (!int.TryParse(parts[0], out int iterations))
            {
                return false; // Invalid iteration count
            }

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedKey = Convert.FromBase64String(parts[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            // Compare byte arrays in a time-constant manner.
            return CryptographicOperations.FixedTimeEquals(storedKey, key);
        }
    }
}
