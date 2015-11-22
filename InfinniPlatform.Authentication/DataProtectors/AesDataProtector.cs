using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Authentication.DataProtectors
{
    internal sealed class AesDataProtector : IDataProtector
    {
        public AesDataProtector(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);

            using (var sha = new SHA256Managed())
            {
                _keyHash = sha.ComputeHash(keyBytes);
            }
        }

        private readonly byte[] _keyHash;

        public byte[] Protect(byte[] data)
        {
            byte[] dataHash;

            using (var sha = new SHA256Managed())
            {
                dataHash = sha.ComputeHash(data);
            }

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = _keyHash;
                aesAlg.GenerateIV();

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aesAlg.IV, 0, 16);

                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var bwEncrypt = new BinaryWriter(csEncrypt))
                            {
                                bwEncrypt.Write(dataHash);
                                bwEncrypt.Write(data.Length);
                                bwEncrypt.Write(data);
                            }
                        }

                        var protectedData = msEncrypt.ToArray();

                        return protectedData;
                    }
                }
            }
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = _keyHash;

                using (var msDecrypt = new MemoryStream(protectedData))
                {
                    var iv = new byte[16];
                    msDecrypt.Read(iv, 0, 16);

                    aesAlg.IV = iv;

                    using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var brDecrypt = new BinaryReader(csDecrypt))
                            {
                                var signature = brDecrypt.ReadBytes(32);
                                var len = brDecrypt.ReadInt32();
                                var data = brDecrypt.ReadBytes(len);

                                byte[] dataHash;

                                using (var sha = new SHA256Managed())
                                {
                                    dataHash = sha.ComputeHash(data);
                                }

                                if (!dataHash.SequenceEqual(signature))
                                {
                                    throw new SecurityException("Signature does not match the computed hash");
                                }

                                return data;
                            }
                        }
                    }
                }
            }
        }
    }
}