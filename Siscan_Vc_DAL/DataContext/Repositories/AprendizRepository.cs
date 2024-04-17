using Microsoft.EntityFrameworkCore;
using Siscan_Vc_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.DataContext.Repositories
{
    internal class AprendizRepository : IGenericRepository<Aprendiz>
    {
        private readonly DbSiscanContext _dbcontext;
        public AprendizRepository(DbSiscanContext contextdb)
        {
            _dbcontext = contextdb;
        }
        public async Task<bool> Delete(string numeroDocumento)
        {
            try
            {
                Aprendiz aprendiz = _dbcontext.Aprendiz.FirstOrDefault(a => a.NumeroDocumentoAprendiz == numeroDocumento);
                _dbcontext.Aprendiz.Remove(aprendiz);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Aprendiz>> GetAll()
        {
            IQueryable<Aprendiz> queryAprendiz = _dbcontext.Aprendiz;
            return queryAprendiz;
        }

        public async Task<Aprendiz> GetById(int id)
        {
            try
            {
                return await _dbcontext.Aprendiz.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Aprendiz modelo)
        {
            try
            {
                _dbcontext.Aprendiz.Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Aprendiz modelo)
        {
            try
            {
                _dbcontext.Aprendiz.Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch {return false; }
        }
    }
}
