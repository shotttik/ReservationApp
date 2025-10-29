using Domain.Enums;

namespace Infrastructure.Extensions
{
    public static class UploadSubFolderExtensions
    {
        public static string GetFolderName(this UploadSubFolder folder)
        {
            return folder switch
            {
                UploadSubFolder.UserProfileMedia => "user-profile-media",
                UploadSubFolder.CompanyMedia => "company-media",
                UploadSubFolder.ReviewMedia => "review-media",
                _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null)
            };
        }
    }

}
