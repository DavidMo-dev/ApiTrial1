namespace ApiTrial1.Commons.Helpers
{
    public class SessionTokenHelper
    {
        public static string GenerateToken()
        {
            var rnd = new Random();
            var letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = "";
            for (int i = 0; i < 128; ++i)
            {
                result += letters[rnd.Next(letters.Length)];
            }
            return result;
        }
    }
}
