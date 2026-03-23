using size.Carrinho.Business.DTOs;
using size.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.ApplicationService.ProcessamentoCheckout.Interfaces
{
    public interface IProcessamentoCheckoutAppService
    {
        Task AdicionarNoCarrinho(string tomadorId, List<string> duplicatasIds);
        Task RemoverDoCarrinho(string tomadorId, List<string> duplicatasIds);
        Task LimparCarrinho(string tomadorId);
        OperacaoDTO Checkout(string tomadorId);
        Task<CarrinhoCompletoDTO> Obter(string tomadorId);
    }
}
