using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace PairOrder
{
    internal static class Helper
    {

        internal static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        internal static string GetBankNiftyExpiryScript(IConfiguration configuration, string scriptName)
        {
            var expiryDate = configuration.GetSection("expiryDate").Value;
            var expiryMonth = configuration.GetSection("expiryMonth").Value;
            var expiryYear = configuration.GetSection("expiryYear").Value;
            return $"{ scriptName}{expiryDate}{expiryMonth.ToUpper()}{expiryYear}";
        }
    }
}
