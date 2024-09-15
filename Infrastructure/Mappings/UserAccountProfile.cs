using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappings
{
    public class UserAccountProfile :Profile
    {
        public UserAccountProfile()
        {
            CreateMap<UserAccount, UserAccountDTO>().ReverseMap();
        }
    }
}
