using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IMediaRepository :IBaseRepository<Media>
    {
        Task<bool> Exists(IEnumerable<int> ids);
    }
}
