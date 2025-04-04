using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    public class GuidGenerator
    {
        private static readonly string _base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string GenerateBase32Guid()
        {
            byte[] guidBytes = Guid.NewGuid().ToByteArray();
            return string.Concat(guidBytes.Take(10).Select(b => _base32Chars[b % 32])); // 10-character short ID
        }
    }
}
