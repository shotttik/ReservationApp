namespace Domain.Interfaces.Services
{
    public interface IMediaCleanupService
    {
        Task CleanupOrphanedMediaAsync();
    }
}
