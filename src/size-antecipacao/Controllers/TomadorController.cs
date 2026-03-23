using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using size.CatalogoRecebiveis.Business.AggregateRoots;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.FichaCadastral.Business.AggregateRoots;
using size.FichaCadastral.Business.Interfaces.Repositories;
using size_antecipacao.DTOs;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TomadorController : ControllerBase
    {

        private readonly ILogger<TomadorController> _logger;
        private readonly ITomadorRepository _tomadorRepository;
        private readonly IDuplicataRepository _duplicataRepository;

        public TomadorController(ILogger<TomadorController> logger, 
                                 ITomadorRepository tomadorRepository,
                                 IDuplicataRepository duplicataRepository   )
        {
            _logger = logger;
            _tomadorRepository = tomadorRepository;
            _duplicataRepository = duplicataRepository;
        }

        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] string id)
        {
            var tomador = _tomadorRepository.ObterPorId(id);
            if(tomador is null)
            {
                return NotFound();
            }

            var duplicatas = _duplicataRepository.Listar(x => x.TomadorId == id);

            return Ok(ToTomadoresDTO(tomador,duplicatas));
        }


        private TomadoresDTO ToTomadoresDTO(Tomador tomador, IEnumerable<Duplicata> duplicatas)
        {
            return new TomadoresDTO()
            {
                Id = tomador.Id,
                Nome = tomador.RazaoSocial,
                Documento = tomador.Documento.Numero,
                Duplicatas = duplicatas.Select(x => new DuplicataDTO()
                {
                    Id = x.Id,
                    Numero = x.Numero,
                    Valor = x.Valor,
                    DataVencimento = x.DataVencimento,
                    Status = x.Status.GetDisplayName(),
                }).ToList()
            };
        }
    }
}
