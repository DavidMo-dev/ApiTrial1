using System.Security.Cryptography.X509Certificates;


namespace ApiTrial1.Commons.Request
{
    public class SetCandidateRquest
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public int CandidateId { get; set; }
        public string CandidateUsername { get; set; }
        public string CandidatePassword { get; set; }
    }
}
