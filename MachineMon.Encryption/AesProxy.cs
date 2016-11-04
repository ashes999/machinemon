using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Encryption
{
    public class AesProxy
    {
        private readonly SecureString SecretKey;

        public AesProxy(string key)
        {
            this.SecretKey = new SecureString();
            foreach (char c in key)
            {
                SecretKey.AppendChar(c);
            }
            this.SecretKey.MakeReadOnly();
        }
    }
}
