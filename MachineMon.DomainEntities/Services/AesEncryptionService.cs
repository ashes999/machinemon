using MachineMon.Core.Domain;
using MachineMon.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Core.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        private const int KeySizeInCharacters = 32; // 256 bytes

        private readonly SecureString secureKey = new SecureString();
        private readonly IGenericRepository repository;

        public AesEncryptionService(IGenericRepository repository, ConfigurationSetting configKey)
        {
            var base64Key = configKey.Value;
            var bytes = Convert.FromBase64String(base64Key);
            var key = Encoding.ASCII.GetString(bytes);

            foreach (var c in key)
            {
                secureKey.AppendChar(c);
            }

            secureKey.MakeReadOnly();

            this.repository = repository;
        }

        public string Encrypt(string input)
        {
            // Encrypt the string to an array of bytes. 
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Key = Encoding.UTF8.GetBytes(secureKey.ConvertToUnsecureString());
                rijndael.GenerateIV();

                // Create a decryptor to perform the stream transform.
                var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for encryption. 
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            // Write all data to the stream.
                            streamWriter.Write(input);
                        }

                        byte[] encrypted = memoryStream.ToArray();
                        var output = Convert.ToBase64String(encrypted);

                        return output;
                    }
                }
            }
        }

        //////////// DERP: needed to store the IV ... per-encryption, ideally
        public string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        /// <summary>
        /// The first time you run the web application, it generates a random key to use to encrypt machine passwords.
        /// This is stored in plaintext in web.config. Since MachineMon is a hosted solution, this is probably secure enough for now.
        /// Note that deleting this will generate a new key, but it will cause decryption to fail for all host passwords.
        /// </summary>
        public static void GenerateSecureKeyIfMissing(IGenericRepository repository)
        {
            var config = repository.GetAll<ConfigurationSetting>().SingleOrDefault(c => c.Setting == "SecureKey");
            if (config == null)
            {
                using (var provider = new System.Security.Cryptography.RNGCryptoServiceProvider())
                {
                    // Key is missing. Regenerate it.
                    var bytes = new byte[KeySizeInCharacters]; // 32 characters = 256 bytes, which is the key size for AES
                    provider.GetBytes(bytes);
                    var key = Convert.ToBase64String(bytes);
                    config = new ConfigurationSetting() { Setting = "SecureKey", Value = key };
                    repository.Insert<ConfigurationSetting>(config);
                }
            }
        }
    }

    public static class SecureStringExtensions
    {
        // https://blogs.msdn.microsoft.com/fpintos/2009/06/12/how-to-properly-convert-securestring-to-string/
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
