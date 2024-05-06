using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IInscripcionTYTService
    {
        Task<bool> Insert(InscripcionTyt model);
        Task<bool> Update(InscripcionTyt model);
        Task<bool> Delete(string cogInscripcion);
        Task<InscripcionTyt> GetForCogInscripcion(string cogInscripcion);
        Task<IQueryable<InscripcionTyt>> GetAll();
    }
}
