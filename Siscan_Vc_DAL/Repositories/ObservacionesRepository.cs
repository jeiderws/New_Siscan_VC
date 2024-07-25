using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class ObservacionesRepository : IGenericRepository<Observacion>
    {
        private readonly DbSiscanContext _dbcontext;
        public ObservacionesRepository( DbSiscanContext dbcontext)
        {
                _dbcontext = dbcontext;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Observacion obser = _dbcontext.Observacions.First(o => o.IdObservacion.ToString() == id);
                _dbcontext.Remove(obser);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch {return false;}
        }

        public async Task<IQueryable<Observacion>> GetAll()
        {
            try
            {
                IQueryable<Observacion> queryobser = _dbcontext.Observacions.Include(o => o.IdSeguimientoNavigation);
                return queryobser;
            }
            catch {return null;}
        }

        public async Task<Observacion> GetForId(string id)
        {
            try
            {
                return await _dbcontext.Observacions.FindAsync(id);
            }
            catch {return null;}
        }

        public async Task<bool> Insert(Observacion model)
        {
            try
            {
                _dbcontext.Observacions.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch {return false;}
        }

        public async Task<bool> Update(Observacion model)
        {
            try
            {
                _dbcontext.Observacions.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false;}
        }
    }
}
