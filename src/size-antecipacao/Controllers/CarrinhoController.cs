using Microsoft.AspNetCore.Mvc;
using size.ApplicationService.ProcessamentoCheckout.Interfaces;
using size.Carrinho.Business.DTOs;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.Core.Communication;
using size.Core.DTOs;
using size_antecipacao.Configurations;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : MainController
    {
        private readonly IProcessamentoCheckoutAppService _processamentoCheckoutAppService;
        private readonly IDuplicataRepository _duplicataRepository;

        public CarrinhoController(INotificador notificador, IProcessamentoCheckoutAppService processamentoCheckoutAppService, IDuplicataRepository duplicataRepository) : base(notificador)
        {
            _processamentoCheckoutAppService = processamentoCheckoutAppService;
            _duplicataRepository = duplicataRepository;
        }

        [HttpPost("inserir-duplicata")]
        public async Task<IActionResult> InserirDuplicata(InserirDuplicata inserirDuplicata)
        {
            await _processamentoCheckoutAppService.AdicionarNoCarrinho(inserirDuplicata.TomadorId, inserirDuplicata.DuplicatasIds);

            if (_notificador.TemNotificacao()) return CustomResponse();

            return CustomResponse(await ToCarrinhoCompletoDTO(inserirDuplicata.TomadorId));
        }

        [HttpPost("remover-duplicata")]
        public async Task<IActionResult> RemoverDuplicata(RemoverDuplicata removerDuplicata)
        {
            await _processamentoCheckoutAppService.RemoverDoCarrinho(removerDuplicata.TomadorId, removerDuplicata.DuplicatasIds);

            if (_notificador.TemNotificacao()) return CustomResponse();

            return CustomResponse(await ToCarrinhoCompletoDTO(removerDuplicata.TomadorId));
        }

        [HttpPost("checkout/{tomadorId}")]
        public async Task<IActionResult> Checkout([FromRoute] string tomadorId)
        {
            var operacao = _processamentoCheckoutAppService.Checkout(tomadorId);
            return CustomResponse(operacao);
        }

        [HttpGet("{tomadorId}")]
        public async Task<IActionResult> ObterCarrinho([FromRoute] string tomadorId)
        {
            var carrinho = await _processamentoCheckoutAppService.Obter(tomadorId);
            return CustomResponse(carrinho);
        }


        private async Task<CarrinhoCompletoDTO> ToCarrinhoCompletoDTO(string tomadorId)
        {
            return await _processamentoCheckoutAppService.Obter(tomadorId);
        }
    }
}
