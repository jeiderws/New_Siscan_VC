using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service
{
    public interface IAprendizService
    {
        Task<bool> Insert(Aprendiz model);
        Task<bool> Update(Aprendiz model);
        Task<bool> Delete(string numDoc);
        Task<Aprendiz> GetForDoc(string numeroDoc);
        Task<IQueryable<Aprendiz>> GetAll();
        Task<Aprendiz> GetForName(string name);
    }
}
