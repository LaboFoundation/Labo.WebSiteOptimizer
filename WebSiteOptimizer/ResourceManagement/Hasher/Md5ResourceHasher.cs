using System;
using System.Security.Cryptography;
using System.Text;

namespace Labo.WebSiteOptimizer.ResourceManagement.Hasher
{
    internal sealed class Md5ResourceHasher : IResourceHasher
    {
        public string HashContent(string content)
        {
            if (content == null)
            {
                throw new InvalidOperationException("Cannot hash null string.");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            return ByteArrayToString(hashBytes);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder output = new StringBuilder(arrInput.Length);
            for (int i = 0; i < arrInput.Length; i++)
            {
                output.Append(arrInput[i].ToString("X2"));
            }
            return output.ToString().ToLowerInvariant();
        }
    }
}