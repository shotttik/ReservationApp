using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserAccountService
    {
        Task AddUserAccountAsync(UserAccountDTO userAccountDTO);
    }
}
