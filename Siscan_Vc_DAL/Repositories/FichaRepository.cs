using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class FichaRepository : IGenericRepository<Ficha>
    { 
        private readonly DbSiscanContext _context;
        public FichaRepository(DbSiscanContext context)
        {

            _context = context;

        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Ficha ficha=_context.Fichas.First(f=>f.Ficha1.ToString()==id);
                _context.Remove(ficha);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Ficha>> GetAll()
        {
            try
            {
                IQueryable<Ficha> queryFicha=_context.Fichas.Include(p=>p.ProgramaNavigation).Include(n=>n.NumeroDocumentoInstructorNavigation).Include(s=>s.IdSedeNavigation);
                return queryFicha;
            }
            catch { return null; }
        }

        public async Task<Ficha> GetForId(string id)
        {
            try
            {
                return await _context.Fichas.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Ficha model)
        {
            try
            {
                _context.Fichas.Add(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Ficha model)
        {
            try
            {
                _context.Fichas.Update(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
