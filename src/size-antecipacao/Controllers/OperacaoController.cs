using Microsoft.AspNetCore.Mvc;
using size.Operacao.Application.Services;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacaoController : ControllerBase
    {
        private readonly OperacaoService _operacaoService;

        public OperacaoController(OperacaoService operacaoService)
        {
            _operacaoService = operacaoService;
        }

        

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            var operacao = await _operacaoService.ObterOperacaoPorCodigo(codigo);

            if (operacao == null)
                return NotFound();

            return Ok(operacao);
        }        
    }    
}
