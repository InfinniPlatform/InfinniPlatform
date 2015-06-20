using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace InfinniPlatform.SystemConfig.Administration.Common
{
    /// <summary>
    ///     Предоставляет методы хэширования.
    /// </summary>
    internal static class StringHasher
    {
        private const int Iterations = 1000;
        private const int SaltLength = 16;
        private const int SubkeyLength = 32;
        private const int HashLength = SaltLength + SubkeyLength + 1;

        /// <summary>
        ///     Вычислить хэш строки.
        /// </summary>
        public static string HashValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            byte[] salt;
            byte[] subkey;

            using (var deriveBytes = new Rfc2898DeriveBytes(value, SaltLength, Iterations))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(SubkeyLength);
            }

            var hashBytes = new byte[HashLength];
            Buffer.BlockCopy(salt, 0, hashBytes, 1, SaltLength);
            Buffer.BlockCopy(subkey, 0, hashBytes, SaltLength + 1, SubkeyLength);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        ///     Проверить, что строка соответствует хэшу.
        /// </summary>
        public static bool VerifyValue(string hash, string value)
        {
            if (hash == null)
            {
                return false;
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var hashBytes = Convert.FromBase64String(hash);

            if (hashBytes.Length != HashLength || hashBytes[0] != 0)
            {
                return false;
            }

            var salt = new byte[SaltLength];
            var subkey = new byte[SubkeyLength];

            Buffer.BlockCopy(hashBytes, 1, salt, 0, SaltLength);
            Buffer.BlockCopy(hashBytes, SaltLength + 1, subkey, 0, SubkeyLength);

            byte[] actualSubkey;

            using (var deriveBytes = new Rfc2898DeriveBytes(value, salt, Iterations))
            {
                actualSubkey = deriveBytes.GetBytes(SubkeyLength);
            }

            return ByteArraysEqual(subkey, actualSubkey);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            var flag = true;

            for (var index = 0; index < left.Length; ++index)
            {
                flag = flag & left[index] == right[index];
            }

            return flag;
        }
    }
}