using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class InscripcionTYTRepository : IGenericRepository<InscripcionTyt>
    {
        private readonly DbSiscanContext _dbcontext;
        public InscripcionTYTRepository(DbSiscanContext dbcontext)
        {

            _dbcontext = dbcontext;

        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                InscripcionTyt instyt = _dbcontext.InscripcionTyts.First(i=> i.CodigoInscripcion==id);
                _dbcontext.Remove(instyt);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<InscripcionTyt>> GetAll()
        {
            try
            {
                IQueryable<InscripcionTyt> queryTYT=_dbcontext.InscripcionTyts.Include(c=>c.IdConvocatoriaNavigation).Include(e=>e.IdEstadotytNavigation).Include(c=>c.IdciudadNavigation).Include(a=>a.NumeroDocumentoAprendizNavigation);
                return queryTYT;
            }
            catch { return null; }
        }

        public async Task<InscripcionTyt> GetForId(string id)
        {
            try
            {
                return await _dbcontext.InscripcionTyts.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(InscripcionTyt model)
        {
            try
            {
                _dbcontext.InscripcionTyts.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(InscripcionTyt model)
        {
            try
            {
                _dbcontext.InscripcionTyts.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
