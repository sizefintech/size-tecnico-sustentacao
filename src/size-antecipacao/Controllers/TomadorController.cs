using Microsoft.AspNetCore.Mvc;
using size.FichaCadastral.Business.Interfaces.Repositories;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TomadorController : ControllerBase
    {

        private readonly ILogger<TomadorController> _logger;
        private readonly ITomadorRepository _tomadorRepository;

        public TomadorController(ILogger<TomadorController> logger, 
                                 ITomadorRepository tomadorRepository)
        {
            _logger = logger;
            _tomadorRepository = tomadorRepository;
        }

        [HttpGet(Name = "Teste")]
        public ActionResult Get()
        {
            var tomadores = _tomadorRepository.Listar(x=> x.RazaoSocial.Contains("teste"));

            return Ok(tomadores);
        }
    }
}
