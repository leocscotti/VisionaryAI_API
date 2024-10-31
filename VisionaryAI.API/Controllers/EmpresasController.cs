using Microsoft.AspNetCore.Mvc;
using VisionaryAI.API.Models;
using VisionaryAI.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisionaryAI.API.DTO;

namespace VisionaryAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;

        public EmpresasController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empresa>>> BuscarTodasEmpresas()
        {
            var empresas = await _empresaService.BuscarTodasEmpresas();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> BuscarPorId(int id)
        {
            var empresa = await _empresaService.BuscarPorId(id);
            if (empresa == null)
            {
                return NotFound($"A empresa com ID {id} não foi encontrada.");
            }
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<ActionResult<Empresa>> Cadastrar([FromBody] Empresa empresa)
        {

            Empresa novaEmpresa = await _empresaService.Adicionar(empresa);
            return Ok(novaEmpresa);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Empresa>> Atualizar([FromBody] Empresa empresa, int id)
        {
            empresa.Id = id;
            try
            {
                var empresaAtualizada = await _empresaService.Atualizar(empresa, id);
                return Ok(empresaAtualizada);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Excluir(int id)
        {
            var empresaExcluida = await _empresaService.Apagar(id);
            if (!empresaExcluida)
            {
                return NotFound($"A empresa com ID {id} não foi encontrada.");
            }
            return NoContent();
        }
    }
}
