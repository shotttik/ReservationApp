namespace Application.Common.Results
{
    public class MediaResults
    {
        #region Errors  
        public static readonly Error InvalidFile = Error.Validation("Media.InvalidFile", "The provided file is not valid or does not meet the required criteria.");
        public static readonly Error ImageTooLarge = Error.Validation("Media.ImageTooLarge", "The uploaded image exceeds the maximum allowed size.");
        public static readonly Error InvalidImageType = Error.Validation("Media.InvalidImageType", "The uploaded image type is not supported. Please upload a valid image file.");
        public static readonly Error SomeMediaDontExists = Error.Validation("Media.SomeMediaDontExists", "Some of the specified media items don't exists.");
        #endregion

        #region Success  
        public static readonly SuccessInfo ImageUploaded = new("Media.ImageUploaded", "The image has been successfully uploaded.");
        public static readonly SuccessInfo ImageDeleted = new("Media.ImageDeleted", "The image has been successfully deleted.");
        public static readonly SuccessInfo ImagesUploaded = new("Media.ImagesUploaded", "The images have been successfully uploaded.");
        public static readonly SuccessInfo ImagesUpdated = new("Media.ImagesUpdated", "The images have been successfully updated.");

        #endregion
    }
}
