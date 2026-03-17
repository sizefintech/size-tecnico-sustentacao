using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.ApplicationService.ProcessamentoCheckout.Interfaces
{
    public interface IProcessamentoCheckoutAppService
    {
        void AdicionarNoCarrinho(string tomadorId, List<string> duplicatasIds);
        void RemoverDoCarrinho(string tomadorId, List<string> duplicatasIds);
        void LimparCarrinho(string tomadorId);
        void Checkout(string tomadorId);

    }
}
