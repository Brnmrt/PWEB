using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;
using RESTfulAPI.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace RESTfulAPI.Controllers;
[Route("api/[Controller]")]
[ApiController]

public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet]

    public async Task<IActionResult> GetProdutos(string tipoProduto, int? searchId = null)
    {
        IEnumerable<Produto> produtos;


        Console.WriteLine("Procura de produtos " + tipoProduto + searchId);
        if (tipoProduto == "categoria" && searchId != null)
        {
            Console.WriteLine("Procura por categoria");
            produtos = await _produtoRepository.
                    ObterProdutosPorCategoriaAsync(searchId.Value);
        }
        else if (tipoProduto == "marca" && searchId != null)
        {
            produtos = await _produtoRepository.
                ObterProdutosPorMarcaAsync(searchId.Value);
        }
        else if (tipoProduto == "classificacao" && searchId != null)
        {
            produtos = await _produtoRepository.
                    ObterProdutosPorClassificacaoAsync(searchId.Value);
        }
        else if (tipoProduto == "promocao")
        {
            var promocoes = await _produtoRepository.
                ObterProdutosPromocaoAsync();
            return Ok(promocoes);
        }
        else if (tipoProduto == "maisvendido")
        {
            produtos = await _produtoRepository.
                ObterProdutosMaisVendidosAsync();
        }
        else if (tipoProduto == "todos")
        {
            produtos = await _produtoRepository.
                ObterTodosProdutosAsync();
        }
        else
        {
            return BadRequest("Tipo de produto Inválido");
        }
        return Ok(produtos);
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetDetalheProduto(int id)
    {
        Produto produto = await _produtoRepository.ObterDetalheProdutoAsync(id);

        if (produto is null)
        {
            return NotFound($"Produto com id = {id} nao encontrado");
        }
        return Ok(produto);
    }

    [HttpGet("[action]")]
    [Authorize(Roles = ("Administrador,Funcionario,Cliente"), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetFavoritosUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var produtos = await _produtoRepository.ObterProdutosFavoritosUserAsync(userId);
        return Ok(produtos);
    }

    [HttpPut("[action]")]
    [Authorize(Roles = ("Administrador,Funcionario,Cliente"), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AdicionaFavoritoUser(int produtoId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool result = await _produtoRepository.AdicionaFavorito(userId, produtoId);
        if(result)
            return StatusCode(StatusCodes.Status201Created);
        else
            return StatusCode(StatusCodes.Status400BadRequest);
    }
    [HttpPut("[action]")]
    [Authorize(Roles = ("Administrador,Funcionario,Cliente"), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveFavoritoUser(int produtoId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        bool result = await _produtoRepository.RemoveFavorito(userId, produtoId);
        if (result)
            return StatusCode(StatusCodes.Status201Created);
        else
            return StatusCode(StatusCodes.Status400BadRequest);
    }
}
