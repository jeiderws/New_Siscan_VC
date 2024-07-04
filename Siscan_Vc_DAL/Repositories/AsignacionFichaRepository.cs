using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class AsignacionFichaRepository : IGenericRepository<AsignacionFicha>
    {
        private readonly DbSiscanContext _contextdb;
        public AsignacionFichaRepository(DbSiscanContext contextdb)
        {
            _contextdb = contextdb;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                AsignacionFicha asignacionFicha = _contextdb.AsignacionFichas.First(a => a.AsignacionFichaId.ToString() == id);
                _contextdb.Remove(asignacionFicha);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }

        }

        public async Task<IQueryable<AsignacionFicha>> GetAll()
        {
            try
            {
                IQueryable<AsignacionFicha> asignacionFichas = _contextdb.AsignacionFichas.Include(a => a.FichaNavigation).Include(e => e.NumeroDocumentoInstructorNavigation);
                return asignacionFichas;
            }
            catch { return null; }
        }

        public async Task<AsignacionFicha> GetForId(string id)
        {
            try
            {
                return await _contextdb.AsignacionFichas.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(AsignacionFicha model)
        {
            try
            {
                _contextdb.AsignacionFichas.Add(model);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(AsignacionFicha model)
        {
            try
            {
                _contextdb.AsignacionFichas.Update(model);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
