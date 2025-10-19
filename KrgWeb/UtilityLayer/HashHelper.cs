using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    public static class HashHelper
    {
        public static string ComputeSha512Hash(string rawData)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();

                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Converts to hex string
                }

                return builder.ToString();
            }
        }
    }
}
