using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.ClasesService
{
    public class FichaService : IFichaService
    { 
        private readonly IGenericRepository<Ficha> _repoFicha;
        public FichaService(IGenericRepository<Ficha> repoFicha)
        {
            _repoFicha=repoFicha;
        }
        public Task<bool> Delete(string ficha)
        {
            return _repoFicha.Delete(ficha);
        }

        public Task<IQueryable<Ficha>> GetAll()
        {
            return _repoFicha.GetAll(); 
        }

        public Task<Ficha> GetForFicha(string ficha)
        {
           return _repoFicha.GetForId(ficha);
        }

        public Task<bool> Insert(Ficha model)
        {
            return _repoFicha.Insert(model);
        }

        public Task<bool> Update(Ficha model)
        {
            return _repoFicha.Update(model);
        }
        public async Task<List<Ficha>> GetByCodigoFicha(string codigoFicha)
        {
            var fichas = await _repoFicha.GetAll(); 

            return fichas.Where(f => f.Ficha1.ToString() == codigoFicha).ToList(); 
        }
        public async Task<Ficha> GetByCodigoPrograma(string codigoPrograma)
        {
            var codpro = await _repoFicha.GetAll();
            return codpro.FirstOrDefault(f => f.CodigoPrograma == codigoPrograma);
        }
    }
}
