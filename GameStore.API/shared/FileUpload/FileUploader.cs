namespace GameStore.API.shared.FileUpload
{
    public class FileUploader(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        /// <summary>
        /// Uploads a file to the specified folder asynchronously.
        /// </summary>
        /// <param name="file">The uploaded file as an <see cref="IFormFile"/>. Intended for small-to-moderate-sized files because <see cref="IFormFile"/> may buffer data in memory; for large files consider a streaming approach (e.g., read the request body stream) to avoid high memory usage.</param>
        /// <param name="folder">Relative destination folder (under the app/web root) where the file will be stored. Must be validated and sanitized to prevent path traversal and ensure the folder exists or is created.</param>
        /// <returns>A <see cref="FileUploadResult"/> containing upload outcome and metadata.</returns>
        public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder)
        {

            FileUploadResult result = new FileUploadResult();

            // Validation

            if (file == null || file.Length == 0)
            {
               result.IsSucess = false;
               result.ErrorMessage = "No File uploaded.";
               return result;
            }

            if (file.Length > 10 * 1024 * 1024) // 10 MB limit
            {
                result.IsSucess = false;
                result.ErrorMessage = "File size exceeds the 10 MB limit.";
                return result;
            }

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                result.IsSucess = false;
                result.ErrorMessage = "File type is not permitted.";
                return result;
            }

            // Upload logic

            var uploadsRootFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            // Generate a unique filename to avoid collisions
            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsRootFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(stream);

            // Generate a publicly accessible URL for the uploaded file by combining the request scheme (http/https),
            // host (domain:port), folder path, and filename. This URL is returned to the client so they can
            // access the file through static file serving (e.g., for displaying images or download links).
            var httpContext = httpContextAccessor.HttpContext;
            var fileUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}/{folder}/{uniqueFileName}";

            // Return URL
            result.IsSucess = true;
            result.FileUrl = fileUrl;
            return result;
        }
    }
}