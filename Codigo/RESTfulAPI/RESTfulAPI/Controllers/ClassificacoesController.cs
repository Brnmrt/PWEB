using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories.IRepositories;
using RESTfulAPI.Repositories.Services;
using Microsoft.AspNetCore.Authorization;


namespace RESTfulAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


//[Authorize]

public class ClassificacoesController : ControllerBase
    {
    private readonly IClassificacaoRepository classificacaoRepository;

    public ClassificacoesController(IClassificacaoRepository classificacaoRepository)
    {
        this.classificacaoRepository = classificacaoRepository;
    }
    [HttpGet]

    public async Task<IActionResult> Get()
    {
        var classificacao = await classificacaoRepository.GetClassificacao();
        return Ok(classificacao);
    }
}

