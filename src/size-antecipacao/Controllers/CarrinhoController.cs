using Microsoft.AspNetCore.Mvc;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {

        [HttpPost("inserir-duplicata")]
        public IActionResult InserirDuplicata()
        {
            return Ok();
        }

        [HttpPost("remover-duplicata")]
        public IActionResult RemoverDuplicata()
        {
            return Ok();
        }

        [HttpPost("checkout/{tomadorId}")]
        public IActionResult Checkout([FromRoute] string tomadorId)
        {
            return Ok();
        }
    }
}
