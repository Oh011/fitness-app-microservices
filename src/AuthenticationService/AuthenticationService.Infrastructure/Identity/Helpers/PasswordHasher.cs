using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Security
{


    internal class Hasher
    {
        private const int SaltSize = 16; // 128-bit
        private const int KeySize = 32;  // 256-bit hash
        private const int Iterations = 100_000; // slow hash

        // ---- For passwords (salted) ----
        public (string Hash, string Salt) HashWithSalt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be empty");

            var saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltBytes);

            string salt = Convert.ToBase64String(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(value, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var hashBytes = pbkdf2.GetBytes(KeySize);
            string hash = Convert.ToBase64String(hashBytes);

            return (hash, salt);
        }

        public bool VerifyWithSalt(string value, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);

            using var pbkdf2 = new Rfc2898DeriveBytes(value, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var hashBytes = pbkdf2.GetBytes(KeySize);
            string hash = Convert.ToBase64String(hashBytes);

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(hash),
                Encoding.UTF8.GetBytes(storedHash)
            );
        }

        // ---- For tokens / one-time codes (no salt needed) ----
        public string HashWithoutSalt(string value)
        {

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be empty");

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyWithoutSalt(string value, string storedHash)
        {
            var hash = HashWithoutSalt(value);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(hash),
                Encoding.UTF8.GetBytes(storedHash)
            );
        }
    }


}
