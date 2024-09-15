using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserAccountService :IUserAccountService
    {
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IMapper mapper;

        public UserAccountService(IUserAccountRepository userAccountRepository, IMapper mapper)
        {
            this.userAccountRepository = userAccountRepository;
            this.mapper = mapper;
        }

        public async Task AddUserAccountAsync(UserAccountDTO userAccountDTO)
        {
            var userAccount = mapper.Map<UserAccount>(userAccountDTO);
            await userAccountRepository.AddAsync(userAccount);
        }
    }
}
