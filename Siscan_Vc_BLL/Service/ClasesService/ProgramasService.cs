using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.ClasesService
{
    public class ProgramasService:IProgramasService
    {
        private readonly IGenericRepository<Programas> _programarepo;
        public ProgramasService(IGenericRepository<Programas> programarepo)
        {
            _programarepo = programarepo;
        }

        public Task<bool> Delete(string codigoPrograma)
        {
            return _programarepo.Delete(codigoPrograma);
        }

        public Task<IQueryable<Programas>> GetAll()
        {
            return _programarepo.GetAll();
        }

        public async Task<Programas> GetForCog(string codigoPrograma)
        {
            return await _programarepo.GetForId(codigoPrograma);
        }

        public async Task<Programas> GetForName(string name)
        {
            try
            {
                IQueryable<Programas> queryPrograma=await _programarepo.GetAll();
                Programas programa=queryPrograma.Where(p=>p.NombrePrograma==name).FirstOrDefault();
                return programa;
            }
            catch { return null; }
        }
        public async Task<Programas> GetByCodigo(string codigo)
        {
            var pro = await _programarepo.GetAll();
             return pro.FirstOrDefault(p => p.CodigoPrograma == codigo);
        }

        public Task<bool> Insert(Programas model)
        {
            return _programarepo.Insert(model);
        }

        public async Task<bool> Update(Programas model)
        {
            return await _programarepo.Update(model);
        }
    }
}
