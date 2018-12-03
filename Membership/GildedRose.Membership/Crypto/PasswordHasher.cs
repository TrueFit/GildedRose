using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GildedRose.Membership.Crypto
{
    public class PasswordHasher
    {
        public string EncryptPassword(string key, string iv, string data)
        {
            string encData = null;
            byte[][] keys = this.GetHashKeys(key, iv);

            try
            {
                encData = this.EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return encData;
        }

        public string DecryptPassword(string key, string iv, string data)
        {
            string decData = null;
            byte[][] keys = this.GetHashKeys(key, iv);

            try
            {
                decData = this.DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return decData;
        }

        private byte[][] GetHashKeys(string key, string iv)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(iv);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private string EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            byte[] encrypted;

            using (AesManaged algorithm = new AesManaged())
            {
                algorithm.Key = key;
                algorithm.IV = iv;

                ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                using (MemoryStream encryptedMemoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                            new CryptoStream(encryptedMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encrypted = encryptedMemoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private string DecryptStringFromBytes_Aes(string cipherTextString, byte[] key, byte[] iv)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream encryptedMemoryStream = new MemoryStream(cipherText))
                {
                    using (CryptoStream cryptoStream =
                            new CryptoStream(encryptedMemoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plaintext = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
