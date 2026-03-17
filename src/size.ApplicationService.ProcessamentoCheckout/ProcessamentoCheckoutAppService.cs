using size.ApplicationService.ProcessamentoCheckout.Interfaces;
using size.Carrinho.Application.Services;
using size.CatalogoRecebiveis.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.ApplicationService.ProcessamentoCheckout
{
    public class ProcessamentoCheckoutAppService : IProcessamentoCheckoutAppService
    {
        private readonly CheckoutService _checkoutService;
        private readonly IDuplicataRepository _duplicataRepository;

        public ProcessamentoCheckoutAppService(CheckoutService checkoutService, 
                                               IDuplicataRepository duplicataRepository)
        {
            _checkoutService = checkoutService;
            _duplicataRepository = duplicataRepository;
        }

        public void AdicionarNoCarrinho(string tomadorId, List<string> duplicatasIds)
        {
            var sucesso = _checkoutService.AdicionarDuplicatas(tomadorId, duplicatasIds).Result;
            return;
        }
        public void RemoverDoCarrinho(string tomadorId, List<string> duplicatasIds)
        {
            var sucesso = _checkoutService.RemoverDuplicatas(tomadorId, duplicatasIds).Result;
            return;
        }

        public void Checkout(string tomadorId)
        {
            var sucesso = _checkoutService.Checkout(tomadorId).Result;
            return;
        }

        public void LimparCarrinho(string tomadorId)
        {
            var sucesso = _checkoutService.LimparCarrinho(tomadorId).Result;
            return;

        }

    }
}
