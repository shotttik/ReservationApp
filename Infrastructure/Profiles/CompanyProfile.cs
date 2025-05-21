using AutoMapper;
using Domain.DTO.Company;
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
