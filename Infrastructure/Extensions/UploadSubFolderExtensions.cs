using Domain.Enums;

namespace Infrastructure.Extensions
{
    public static class UploadSubFolderExtensions
    {
        public static string GetFolderName(this UploadSubFolder folder)
        {
            return folder switch
            {
                UploadSubFolder.UserProfilePictures => "user-profile-pictures",
                UploadSubFolder.CompanyImages => "company-images",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };
        }
    }

}
