using Microsoft.AspNetCore.Mvc;
using VisionaryAI.API.Services;
using VisionaryAI.API.Models;
using VisionaryAI.API.Database;

[ApiController]
[Route("api/[controller]")]
public class MLController : ControllerBase
{
    private readonly MLModelService _mlService;
    private readonly VisionaryAIDBContext _dbContext;

    public MLController(MLModelService mlService, VisionaryAIDBContext dbContext)
    {
        _mlService = mlService;
        _dbContext = dbContext;
        _mlService.InicializarModelo(dbContext);
    }

    [HttpPost("prever")]
    public ActionResult<string> Prever([FromBody] FonteDadosInput input)
    {
        var mensagemPrevisao = _mlService.PreverTendencia(input);
        return Ok(mensagemPrevisao);
    }
}
