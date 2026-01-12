using System;

namespace GameStore.API.shared.FileUpload
{
    /// <summary>
    /// Result information returned by the file upload service.
    /// </summary>
    public class FileUploadResult
    {
        /// <summary>
        /// True when the upload completed successfully; otherwise false.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Public URL where the uploaded file can be accessed; may be null on failure.
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// Error message describing why the upload failed; null when the upload succeeded.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}