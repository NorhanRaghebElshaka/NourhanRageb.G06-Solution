﻿using NourhanRageb.G06.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NourhanRageb.G06.BLL.interfaces
{
    public interface IEmployeeRepository :IGeneraicRepository<Employee>
    {
         public Task<IEnumerable<Employee>> GetByNameAsync(string name);

        //IEnumerable<Employee> GetAll(); 
        //Employee Get(int? id); 
        //int Add(Employee employee);
        //int Update(Employee employee);
        //int Delete(Employee employee);
    }
}
