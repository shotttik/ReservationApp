using Domain.Entities.CompanyReleated;
using Domain.Entities.ReviewReleated;
using Domain.Entities.User;

namespace Domain.Entities.Common
{
    public class Media :BaseEntity
    {
        public string OriginalName { get; set; } = default!;       // original file name
        public string RemoteUrl { get; set; } = default!; // path in storage (disk/S3/etc.)
        public string FileType { get; set; } = default!;         // image/jpeg, application/pdf etc.
        public long FileSizeInBytes { get; set; } = default!;             // in bytes
        public ICollection<CompanyMedia> CompanyMedia { get; set; } = [];
        public ICollection<ReviewMedia> ReviewMedia { get; set; } = [];
        public ICollection<UserAccountMedia> UserAccountMedia { get; set; } = [];
    }
}
