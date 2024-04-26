using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IInstructorService
    {
        Task<bool> Insert(Instructor model);
        Task<bool> Update(Instructor model);
        Task<bool> Delete(string numDoc);
        Task<Instructor> GetForDoc(string numeroDoc);
        Task<IQueryable<Instructor>> GetAll();
        Task<Instructor> GetForName(string name);
    }
}

