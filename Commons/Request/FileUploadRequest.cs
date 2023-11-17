namespace ApiTrial1.Commons.Request
{
    public class FileUploadRequest
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public IFormFile File { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
    }
}
