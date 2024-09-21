using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserAccountService :IUserAccountService
    {
        private readonly IUserAccountRepository userAccountRepository;

        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            this.userAccountRepository = userAccountRepository;
        }

        public async Task AddUserAccountAsync(AddUserAccountRequest userAccountDTO)
        {
            //await userAccountRepository.AddAsync(userAccount);
        }
    }
}
