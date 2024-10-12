using NourhanRageb.G06.BLL.interfaces;
using NourhanRageb.G06.BLL.Repositories;
using NourhanRageb.G06.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NourhanRageb.G06.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IEmployeeRepository   _employeeRepository;
        private IDepartmentRepository _departmentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _employeeRepository = new EmployeeRepository(context);
            _departmentRepository = new DepartmentRepository(context);
        }
        public IEmployeeRepository employeeRepository => _employeeRepository;
        public IDepartmentRepository departmentRepository => _departmentRepository;
    }
}
