namespace ApiTrial1.Commons.Request
{
    public class FileUploadRequest
    {
        public string Username;
        public string Token;
        public IFormFile File;
        public int FileId;
        public string FileName;
    }
}
