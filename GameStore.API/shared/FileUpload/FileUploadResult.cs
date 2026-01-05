namespace GameStore.API.shared.FileUpload
{
    public class FileUploadResult
    {
        public bool IsSucess { get; set; }
        public string? FileUrl { get; set; }
        public string? ErrorMessage { get; set; }
    }
}