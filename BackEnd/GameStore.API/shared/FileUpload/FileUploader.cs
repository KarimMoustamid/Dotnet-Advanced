// filepath: /Users/karimmoustamid/Desktop/Dotnet-Advanced/GameStore.API/shared/FileUpload/FileUploader.cs
namespace GameStore.API.shared.FileUpload
{
    /// <summary>
    /// Service responsible for saving uploaded files to disk and producing a public URL for the saved file.
    /// </summary>
    /// <param name="webHostEnvironment">The hosting environment used to resolve the application's web root path (typically <c>wwwroot</c>).</param>
    /// <param name="httpContextAccessor">Accessor used to read the current <c>HttpContext</c> so a public file URL can be built from the current request (scheme + host).</param>
    public class FileUploader(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        /// <summary>
        /// Uploads a file to the specified folder asynchronously.
        /// </summary>
        /// <param name="file">The uploaded file as an <see cref="IFormFile"/>; may be null when the client didn't send a file. Intended for small-to-moderate-sized files because <see cref="IFormFile"/> may buffer data in memory; for large files consider a streaming approach (e.g., read the request body stream) to avoid high memory usage.</param>
        /// <param name="folder">Relative destination folder (under the app/web root) where the file will be stored. Must be validated and sanitized to prevent path traversal and ensure the folder exists or is created.</param>
        /// <returns>A <see cref="FileUploadResult"/> containing upload outcome and metadata.</returns>
        public async Task<FileUploadResult> UploadFileAsync(IFormFile? file, string folder)
        {

            // Create the result object we will return to the caller. Populate IsSuccess/FileUrl/ErrorMessage as we go.
            FileUploadResult result = new FileUploadResult();

            #region Validation

            // Null or empty check: covers cases where no file was uploaded or an empty upload occurred.
            if (file == null || file.Length == 0)
            {
               result.IsSuccess = false;
               result.ErrorMessage = "No File uploaded.";
               return result;
            }

            // Size check: protect the server from very large uploads (here: 10 MB limit).
            if (file.Length > 10 * 1024 * 1024) // 10 MB limit
            {
                result.IsSuccess = false;
                result.ErrorMessage = "File size exceeds the 10 MB limit.";
                return result;
            }

            // Extension/type check: use the client filename's extension as a basic filter.
            // Note: extension checks are not bulletproof—consider validating file signatures for higher security.
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx" };
            // Extract extension and normalize. Path.GetExtension operates on the filename only.
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                result.IsSuccess = false;
                result.ErrorMessage = "File type is not permitted.";
                return result;
            }

            #endregion

            // Compute destination directory under the configured web root and ensure it exists.
            var uploadsRootFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            // Generate a unique filename to avoid collisions and preserve the original extension.
            // Using a GUID avoids relying on the client-provided filename (which can be unsafe).
            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsRootFolder, uniqueFileName);

            // Stream the file to disk. CopyToAsync avoids buffering the entire file in memory.
            using (var stream = new FileStream(filePath, FileMode.Create)) await file.CopyToAsync(stream);

            // Build a public URL for the uploaded file based on the current request's scheme and host.
            // HttpContext may be null in some hosting scenarios; the null-conditional operator avoids exceptions.
            var httpContext = httpContextAccessor.HttpContext;
            var fileUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}/{folder}/{uniqueFileName}";

            // Populate and return success result.
            result.IsSuccess = true;
            result.FileUrl = fileUrl;
            return result;
        }
    }
}