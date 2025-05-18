using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Infrastructure.Profiles
{
    internal class CompanyProfile :Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDTO>();
        }
    }
}
