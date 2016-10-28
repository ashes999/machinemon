using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Core.Services
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts a string value. Returns the base64-encoded output. It's up to the service to fetch
        /// and securely store the app key as needed.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Encrypt(string input);
    }
}
