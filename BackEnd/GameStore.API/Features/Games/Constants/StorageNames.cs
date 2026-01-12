namespace GameStore.API.Features.Games.Constants
{
    /// <summary>
    /// Common storage-related constant names used by the Games feature.
    /// </summary>
    /// <remarks>
    /// Keep folder and blob container names here so callers elsewhere in the codebase
    /// use a single canonical value when storing or serving files related to games.
    /// </remarks>
    public class StorageNames
    {
        /// <summary>
        /// Name of the folder (under the web root) where game images are stored.
        /// Used when saving uploaded images and when building public URLs to those images.
        /// </summary>
        public const string GameImagesFolder = "GameImages";
    }
}