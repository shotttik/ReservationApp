using Domain.Entities.CompanyReleated;

namespace Domain.Entities.Common
{
    public class Media :BaseEntity
    {
        public string FileName { get; set; } = default!;       // original file name
        public string FilePath { get; set; } = default!; // path in storage (disk/S3/etc.)
        public string FileType { get; set; } = default!;         // image/jpeg, application/pdf etc.
        public long FileSize { get; set; } = default!;             // in bytes
        public ICollection<CompanyMedia> CompanyMedias { get; set; } = [];
    }
}
