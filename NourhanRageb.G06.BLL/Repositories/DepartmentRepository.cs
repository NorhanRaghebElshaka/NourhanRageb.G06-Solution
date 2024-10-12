using NourhanRageb.G06.BLL.interfaces;
using NourhanRageb.G06.DAL.Data.Contexts;
using NourhanRageb.G06.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NourhanRageb.G06.BLL.Repositories
{
    public class DepartmentRepository :GeneraicRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
           
        }
    }
}
