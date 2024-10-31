using Microsoft.AspNetCore.Mvc;
using VisionaryAI.API.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MLController : ControllerBase
{
    private readonly MLModelService _mlService;

    public MLController(MLModelService mlService)
    {
        _mlService = mlService ?? throw new ArgumentNullException(nameof(mlService));
    }

    [HttpGet("prever-tendencia/{fonteDadosId}")]
    public async Task<IActionResult> PreverTendencia(int fonteDadosId)
    {
        try
        {
            var resultado = await _mlService.PreverTendenciaPorId(fonteDadosId);
            return Ok(new { Mensagem = resultado });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
    }
}
