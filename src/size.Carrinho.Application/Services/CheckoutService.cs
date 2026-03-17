using size.Carrinho.Business.Interfaces.Repositories;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.Core.Communication;

namespace size.Carrinho.Application.Services
{
    public class CheckoutService
    {
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IDuplicataRepository _duplicataRepository;
        private readonly INotificador _notificador;


        public CheckoutService(
            ICarrinhoRepository carrinhoRepository,
            IDuplicataRepository duplicataRepository,
            INotificador notificador)
        {
            _carrinhoRepository = carrinhoRepository;
            _duplicataRepository = duplicataRepository;
            _notificador = notificador;
        }


        public async Task IniciarProcessamento(string tomadorId)
        {
            try
            {
                var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);
                if (carrinho is null)
                    return;

                carrinho.IniciarProcessamento();
                _carrinhoRepository.Atualizar(carrinho);
                _carrinhoRepository.SalvarAlteracoes();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task FinalizarProcessamento(string tomadorId)
        {
            try
            {
                var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);
                if (carrinho is null)
                    return;

                carrinho.FinalizarProcessamento();
                _carrinhoRepository.Atualizar(carrinho);
                _carrinhoRepository.SalvarAlteracoes();

            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<bool> AdicionarDuplicatas(string tomadorId, List<string> duplicatasIds)
        {

            var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);

            if (carrinho == null)
            {
                carrinho = new Business.AggregateRoots.Carrinho(tomadorId);
                _carrinhoRepository.Adicionar(carrinho);
                _carrinhoRepository.SalvarAlteracoes();
            }

            MarcarComoNoCarrinho(duplicatasIds);
            InserirDuplicatasNoCarrinho(duplicatasIds, carrinho);

            return !PossuiNotificacoes();
        }      

        public async Task<bool> RemoverDuplicatas(string tomadorId, List<string> duplicatasIds)
        {
            var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);

            if (carrinho == null)
            {
                carrinho = new Business.AggregateRoots.Carrinho(tomadorId);
                _carrinhoRepository.Adicionar(carrinho);
            }

            MarcarComoForaDoCarrinho(duplicatasIds);
            RemoverDuplicataNoCarrinho(duplicatasIds, carrinho);

            return !PossuiNotificacoes();
        }

        public async Task<bool> LimparCarrinho(string tomadorId)
        {
            var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);
            if (carrinho == null)
            {
                _notificador.Notificar("Carrinho não encontrado");
                return false;
            }
            var duplicatasIds = carrinho.Duplicatas.Select(d => d.Id).ToList();
            MarcarComoForaDoCarrinho(duplicatasIds);
            RemoverDuplicataNoCarrinho(duplicatasIds, carrinho);
            return !PossuiNotificacoes();
        }

        public async Task<Business.AggregateRoots.Carrinho> ObterCarrinho(string tomadorId)
        {
            var carrinho = await _carrinhoRepository.ObterPorAgregateId(tomadorId);

            if (carrinho == null)
            {
                _notificador.Notificar("Carrinho não encontrado");
                return null;
            }

            

            if (carrinho.EstaProcessando())
            {
                _notificador.Notificar("Carrinho já está em processamento");
                return null;
            }

            return carrinho;
        }




        #region Métodos Privados

        private void FinalizarProcessamentoCarrinho(Business.AggregateRoots.Carrinho carrinho)
        {
            carrinho.FinalizarProcessamento();
            _carrinhoRepository.Atualizar(carrinho);
            _carrinhoRepository.SalvarAlteracoes();
        }

        private void IniciarProcessamentoCarrinho(Business.AggregateRoots.Carrinho carrinho)
        {
            carrinho.IniciarProcessamento();
            _carrinhoRepository.Atualizar(carrinho);
            _carrinhoRepository.SalvarAlteracoes();
        }

        private void InserirDuplicatasNoCarrinho(List<string> duplicatasIds, Business.AggregateRoots.Carrinho carrinho)
        {
            carrinho.InserirDuplicatas(duplicatasIds);
            _carrinhoRepository.Atualizar(carrinho);
            _carrinhoRepository.SalvarAlteracoes();
        }

        private void MarcarComoNoCarrinho(List<string> duplicatasIds)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(duplicatasIds).ToList();
            duplicatas.ForEach(x => x.MarcarComoNoCarrinho());
            _duplicataRepository.SalvarAlteracoes();
        }

        private decimal CalcularTaxaAntecipacao(DateTime dataVencimento)
        {
            var diasParaVencimento = (dataVencimento - DateTime.Now).Days;

            if (diasParaVencimento <= 30)
                return 0.02m;
            else if (diasParaVencimento <= 60)
                return 0.035m;
            else if (diasParaVencimento <= 90)
                return 0.05m;
            else
                return 0.08m;
        }

        private bool PossuiNotificacoes()
        {
            return _notificador.TemNotificacao();
        }

        private void RemoverDuplicataNoCarrinho(List<string> duplicatasIds, Business.AggregateRoots.Carrinho carrinho)
        {
            carrinho.RemoverDuplicatas(duplicatasIds);
            _carrinhoRepository.SalvarAlteracoes();
        }

        private void MarcarComoForaDoCarrinho(List<string> duplicatasIds)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(duplicatasIds).ToList();
            duplicatas.ForEach(x => x.RemoverDoCarrinho());
            _duplicataRepository.SalvarAlteracoes();
        }

        #endregion

    }
}
