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

        public bool CriarOperacao(string tomadorId, List<string> duplicatasIds)
        {
            if (!duplicatasIds.Any())
            {
                _notificador.Notificar("Não é possível criar uma operação sem duplicatas");
                return false;
            }

            var duplicatasCatalogo = ObterDuplicatas(duplicatasIds);
            
            if (!duplicatasCatalogo.Any() || PossuiNotificacoes())
                return false;

            var taxaAntecipacao = CalcularTaxaAntecipacao(duplicatasCatalogo);
            var prazoMedio = CalcularPrazoMedio(duplicatasCatalogo);
            var codigo = GerarCodigoOperacao();

            var operacao = new Business.Entities.Operacao(tomadorId, codigo, taxaAntecipacao, prazoMedio);

            AdicionarDuplicatasNaOperacao(operacao, duplicatasCatalogo);

            _operacaoRepository.Adicionar(operacao);
            _operacaoRepository.SalvarAlteracoes();

            return !PossuiNotificacoes();
        }

        public async Task<bool> ProcessarOperacao(string operacaoId)
        {
            var operacao = await _operacaoRepository.ObterOperacaoComDuplicatas(operacaoId);

            if (operacao == null)
            {
                _notificador.Notificar("Operação não encontrada");
                return false;
            }

            try
            {
                operacao.ProcessarOperacao();
                _operacaoRepository.Atualizar(operacao);
                _operacaoRepository.SalvarAlteracoes();

                return !PossuiNotificacoes();
            }
            catch (Exception ex)
            {
                _notificador.Notificar(ex.Message);
                return false;
            }
        }

        public async Task<Business.Entities.Operacao> ObterOperacaoPorCodigo(string codigo)
        {
            return await _operacaoRepository.ObterPorCodigo(codigo);
        }

        public async Task<IEnumerable<Business.Entities.Operacao>> ObterOperacoesPorTomador(string tomadorId)
        {
            return await _operacaoRepository.ObterPorTomadorId(tomadorId);
        }

        private List<CatalogoRecebiveis.Business.AggregateRoots.Duplicata> ObterDuplicatas(List<string> duplicatasIds)
        {
            var duplicatas = new List<CatalogoRecebiveis.Business.AggregateRoots.Duplicata>();

            foreach (var duplicataId in duplicatasIds)
            {
                var duplicata = _duplicataRepository.ObterPorId(duplicataId);
                
                if (duplicata == null)
                {
                    _notificador.Notificar($"Duplicata {duplicataId} não encontrada");
                    continue;
                }

                if (duplicata.Status != 0)
                {
                    _notificador.Notificar($"Duplicata {duplicata.Numero} não está disponível");
                    continue;
                }

                duplicatas.Add(duplicata);
            }

            return duplicatas;
        }

        private void AdicionarDuplicatasNaOperacao(
            Business.Entities.Operacao operacao,
            List<CatalogoRecebiveis.Business.AggregateRoots.Duplicata> duplicatasCatalogo)
        {
            var duplicatasOperacao = duplicatasCatalogo.Select(d => 
                new Business.Entities.Duplicata(
                    d.Numero,
                    d.DataVencimento,
                    d.Valor,
                    operacao.Id
                )).ToList();

            operacao.AdicionarDuplicatas(duplicatasOperacao);
        }

        private decimal CalcularTaxaAntecipacao(List<CatalogoRecebiveis.Business.AggregateRoots.Duplicata> duplicatas)
        {
            var prazoMedio = CalcularPrazoMedio(duplicatas);

            if (prazoMedio <= 30)
                return 0.02m;
            else if (prazoMedio <= 60)
                return 0.035m;
            else if (prazoMedio <= 90)
                return 0.05m;
            else
                return 0.08m;
        }

        private int CalcularPrazoMedio(List<CatalogoRecebiveis.Business.AggregateRoots.Duplicata> duplicatas)
        {
            var somaValoresPonderados = 0m;
            var somaValores = 0m;

            foreach (var duplicata in duplicatas)
            {
                var diasParaVencimento = (duplicata.DataVencimento - DateTime.Now).Days;
                somaValoresPonderados += duplicata.Valor * diasParaVencimento;
                somaValores += duplicata.Valor;
            }

            return somaValores > 0 ? (int)(somaValoresPonderados / somaValores) : 0;
        }

        private string GerarCodigoOperacao()
        {
            return $"OP-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private bool PossuiNotificacoes()
        {
            return _notificador.TemNotificacao();
        }
    }
}
