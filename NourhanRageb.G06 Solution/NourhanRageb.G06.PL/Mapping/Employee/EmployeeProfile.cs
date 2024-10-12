using AutoMapper;
using NourhanRageb.G06.DAL.Models;
using NourhanRageb.G06.PL.ViewModels;

namespace NourhanRageb.G06.PL.Mapping.Employees
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
