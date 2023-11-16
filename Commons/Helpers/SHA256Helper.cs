using System.Text;

namespace ApiTrial1.Commons.Helpers
{
    public class SHA256Helper
    {
        public static string SHA256(string cadena)
        {
            var bytes = Encoding.UTF8.GetBytes(cadena);
            bytes = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes);
            var SHAresultStr = BitConverter.ToString(bytes).Replace("-", "");
            return SHAresultStr;
        }
    }
}
