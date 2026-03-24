using size.CatalogoRecebiveis.Business.Interfaces;
using size.Core.Communication;
using size.Operacao.Business.Interfaces.Repositories;

namespace size.Operacao.Application.Services
{
    public class OperacaoService
    {
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IDuplicataRepository _duplicataRepository;
        private readonly INotificador _notificador;

        public OperacaoService(
            IOperacaoRepository operacaoRepository,
            IDuplicataRepository duplicataRepository,
            INotificador notificador)
        {
            _operacaoRepository = operacaoRepository;
            _duplicataRepository = duplicataRepository;
            _notificador = notificador;
        }        

        public async Task<Business.AggregateRoots.Operacao> ObterOperacaoPorCodigo(string codigo)
        {
            return await _operacaoRepository.ObterPorCodigo(codigo);
        }       
       
        public async Task<Business.AggregateRoots.Operacao> ObterPorId(string id)
        {
            return await _operacaoRepository.ObterPorId(id);
        }

        public async Task<Business.AggregateRoots.Operacao> ObterResumoPorId(string id)
        {
            return await _operacaoRepository.ObterOperacaoComDuplicatas(id);
        }

    }
}
