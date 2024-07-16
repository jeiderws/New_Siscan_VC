using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IFichaService
    {
        Task<bool> Insert(Ficha model);
        Task<bool> Update(Ficha model);
        Task<bool> Delete(string ficha);
        Task<Ficha> GetForFicha(string ficha);
        Task<IQueryable<Ficha>> GetAll();
        Task<List<Ficha>> GetByCodigoFicha(string codigoFicha);
        Task<Ficha> GetByCodigoPrograma(string codigoPrograma);
    }
}
