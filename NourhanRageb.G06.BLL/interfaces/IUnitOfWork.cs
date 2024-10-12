using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NourhanRageb.G06.BLL.interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository employeeRepository { get; }
        public IDepartmentRepository  departmentRepository{ get; }
    }
}
