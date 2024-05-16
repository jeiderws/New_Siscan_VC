using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IAreasService
    {
        Task<bool> Insert(AreasEmpresa model);
        Task<bool> Update(AreasEmpresa model);
        Task<bool> Delete(int id);
        Task<AreasEmpresa> GetForId(int id);
        Task<IQueryable<AreasEmpresa>> GetAll();
        Task<AreasEmpresa> GetForName(string name);
    }
}
