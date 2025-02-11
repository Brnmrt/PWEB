using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Data;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;

namespace RESTfulAPI.Repositories.Services;

public class EncomendaRepository : IEncomendaRepository
{

    private readonly ApplicationDbContext dbContext;

    public EncomendaRepository(ApplicationDbContext context)
    {
        this.dbContext = context;
    }
    public async Task<IEnumerable<Encomenda>> GetEncomendas()
    {
        var encomendas = await dbContext.Encomendas
            .Include(e => e.User)
            .Include(e => e.Produto)
            .Include(e => e.Produto.marca)
            .Include(e => e.Produto.classificacao)
            .Include(e => e.Produto.categoria)
            .ToListAsync();

        if(encomendas is not null)
        {
            foreach (var encomenda in encomendas)
            {
                encomenda.EmStock = dbContext.Produtos.Find(encomenda.ProdutoId).EmStock > 0;
                dbContext.Entry(encomenda).Property(e => e.EmStock).IsModified = true;
            }
        }
        await dbContext.SaveChangesAsync();
        return encomendas;
    }

    public async Task<IEnumerable<Encomenda>> GetEncomendasByUserId(string userId)
    {
        var encomendas = await dbContext.Encomendas
            .Include(e => e.User)
            .Include(e => e.Produto)
            .Include(e => e.Produto.marca)
            .Include(e => e.Produto.classificacao)
            .Include(e => e.Produto.categoria)
            .Where(e => e.User.Id == userId)
            .ToListAsync();

        if (encomendas is not null)
        {
            foreach (var encomenda in encomendas)
            {
                encomenda.EmStock = dbContext.Produtos.Find(encomenda.ProdutoId).EmStock > 0;
                dbContext.Entry(encomenda).Property(e => e.EmStock).IsModified = true;
            }
        }
        await dbContext.SaveChangesAsync();
        return encomendas;
    }

    public async Task AddEncomenda(Encomenda encomenda)
    {
        if (encomenda == null)
            throw new ArgumentNullException(nameof(encomenda));

        // Buscar o usuário pelo UserId
        var user = await dbContext.Users.FindAsync(encomenda.UserId);
        if (user == null)
        {
            throw new ArgumentException("Usuário não encontrado.");
        }
        var produto = await dbContext.Produtos.FindAsync(encomenda.ProdutoId);
        // Associar o usuário à encomenda
        encomenda.User = user;
        encomenda.Produto = produto;
        encomenda.Paga = false;
        encomenda.EmStock = true;

        // Adicionar a encomenda ao banco de dados
        await dbContext.Encomendas.AddAsync(encomenda);
        await dbContext.SaveChangesAsync();
    }

    private bool EncomendaExists(int id)
    {
        return dbContext.Encomendas.Any(e => e.Id == id);
    }

    public async Task PagaEncomenda(int encomendaId)
    {
        var encomenda = await dbContext.Encomendas.FindAsync(encomendaId);
        if (encomenda == null)
        {
            throw new ArgumentException("Encomenda não encontrada.");
        }

        encomenda.Paga = true;
        dbContext.Encomendas.Update(encomenda);
        await dbContext.SaveChangesAsync();
    }





}

