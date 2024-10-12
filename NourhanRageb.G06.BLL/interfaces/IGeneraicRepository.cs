using NourhanRageb.G06.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NourhanRageb.G06.BLL.interfaces
{
    public interface IGeneraicRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(); 
        Task<T> GetAsync(int? id); 

        Task<int> AddAsync(T Entity);
        Task<int> UpdateAsync(T Entity);
        Task<int> DeleteAsync(T Entity);
    }
}
