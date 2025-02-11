using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;

namespace RESTfulAPI.Repositories.Services
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MarcaRepository(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        public async Task<IEnumerable<Marca>> GetMarcas()
        {
            return await dbContext.Marcas
                .Where(x => x.Imagem.Length > 0)
                .OrderBy(O => O.Ordem)
                .ThenBy(p => p.Nome)
                .ToListAsync();
        }
    }
}
