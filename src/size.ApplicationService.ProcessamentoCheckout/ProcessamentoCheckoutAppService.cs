using size.ApplicationService.ProcessamentoCheckout.Interfaces;
using size.Carrinho.Application.Services;
using size.CatalogoRecebiveis.Business.Interfaces;
using size.Core.Communication;
using size.Operacao.Business.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using size.Operacao.Business.Entities;
using size.Carrinho.Business.DTOs;
using size.Core.DTOs;

namespace size.ApplicationService.ProcessamentoCheckout
{
    public class ProcessamentoCheckoutAppService : IProcessamentoCheckoutAppService
    {
        private readonly CheckoutService _checkoutService;
        private readonly IDuplicataRepository _duplicataRepository;
        private readonly IOperacaoRepository _operacaoRepository;
        private INotificador _notificador;

        public ProcessamentoCheckoutAppService(CheckoutService checkoutService,
                                               IDuplicataRepository duplicataRepository,
                                               IOperacaoRepository operacaoRepository,
                                               INotificador notificador)
        {
            _checkoutService = checkoutService;
            _duplicataRepository = duplicataRepository;
            _operacaoRepository = operacaoRepository;
            _notificador = notificador;
        }

        public async Task AdicionarNoCarrinho(string tomadorId, List<string> duplicatasIds)
        {
            try
            {
                ValidarDuplicatas(duplicatasIds);
                if (!OperacaoValida())
                    return;

                await _checkoutService.AdicionarDuplicatas(tomadorId, duplicatasIds);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task LimparCarrinho(string tomadorId)
        {
            var sucesso = await _checkoutService.LimparCarrinho(tomadorId);
            return;

        }

        public async Task<CarrinhoCompletoDTO> Obter(string tomadorId)
        {
            var carrinho = await _checkoutService.ObterCarrinho(tomadorId);
            if (carrinho == null)
            {
                _notificador.Notificar("Carrinho não encontrado");
                return null;
            }

            return ToCarrinhoCompletoDTO(carrinho);
        }

        private CarrinhoCompletoDTO ToCarrinhoCompletoDTO(Carrinho.Business.AggregateRoots.Carrinho carrinho)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(carrinho.Duplicatas.Select(d => d.Id).ToList());

            return new CarrinhoCompletoDTO
            {
                TomadorId = carrinho.AgregateId,
                Duplicatas = duplicatas.Select(d => new DuplicataCarrinhoDTO
                {
                    Id = d.Id,
                    Valor = d.Valor,
                    Vencimento = d.DataVencimento,

                }).ToList(),
                Valor = duplicatas.Sum(d => d.Valor),
            };
        }

        #region Métodos Privados


        public async Task RemoverDoCarrinho(string tomadorId, List<string> duplicatasIds)
        {
            var sucesso = await _checkoutService.RemoverDuplicatas(tomadorId, duplicatasIds);
            return;
        }

        public OperacaoDTO Checkout(string tomadorId)
        {
            try
            {
                var carrinho = _checkoutService.ObterCarrinho(tomadorId).Result;
                if (carrinho is null)
                {
                    return null;
                }

                ValidarCheckout(carrinho);
                if (!OperacaoValida())
                    return null;

                var duplicatasIds = carrinho.Duplicatas.Select(d => d.Id).ToList();

                var operacao = CriarOperacao(tomadorId, carrinho);
                MarcarDuplicatasComoOperada(duplicatasIds);
                _checkoutService.FinalizarProcessamento(tomadorId).Wait();

                return new OperacaoDTO
                {
                    TomadorId = tomadorId,
                    Id = operacao.Id,
                    AtualizadoEm = operacao.AtualizadoEm,
                    Codigo = operacao.Codigo,
                    CriadoEm = operacao.CriadoEm,
                    DataCriacao = operacao.DataCriacao,
                    DataProcessamento = operacao.DataProcessamento,
                    Prazo = operacao.Prazo,
                    TaxaAntecipacao = operacao.TaxaAntecipacao,
                    ValorBruto = operacao.ValorBruto,
                    ValorLiquido = operacao.ValorLiquido,
                    Duplicatas = ToDuplicatasDTO(duplicatasIds)
                };
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        private List<DuplicataOperacaoDTO> ToDuplicatasDTO(List<string> duplicatasIds)
        {
            var lista = new List<DuplicataOperacaoDTO>();
            var duplicatas = _duplicataRepository.ListarPorIds(duplicatasIds);

            if (duplicatasIds is null || !duplicatasIds.Any()) return lista; 

            foreach (var duplicata in duplicatas)
            {
                lista.Add(new DuplicataOperacaoDTO
                {
                    Id = duplicata.Id,
                    Numero = duplicata.Numero,
                    Vencimento = duplicata.DataVencimento,
                    Valor = duplicata.Valor
                });
            }

            return lista;
        }

        private Operacao.Business.AggregateRoots.Operacao CriarOperacao(string tomadorId, Carrinho.Business.AggregateRoots.Carrinho carrinho)
        {
            var operacao = new Operacao.Business.AggregateRoots.Operacao(tomadorId, ObterDuplicatas(carrinho.Duplicatas.Select(d => d.Id).ToList()));
            _operacaoRepository.Adicionar(operacao);
            _operacaoRepository.SalvarAlteracoes();

            return operacao;
        }

        private void MarcarDuplicatasComoOperada(List<string> list)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(list);

            duplicatas.ToList().ForEach(d => { d.MarcarComoOperada(); d.RemoverDoCarrinho(); });
            _duplicataRepository.Atualizar(duplicatas);
            _duplicataRepository.SalvarAlteracoes();

        }

        private List<Operacao.Business.Entities.Duplicata> ObterDuplicatas(List<string> duplicatasIds)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(duplicatasIds);
            if (duplicatas == null || !duplicatas.Any())
            {
                _notificador.Notificar("Nenhuma duplicata encontrada para os IDs fornecidos");
                return null;
            }
            if (duplicatas.Count() != duplicatasIds.Count)
            {
                _notificador.Notificar("Algumas duplicatas não foram encontradas para os IDs fornecidos");
                return null;
            }

            var lista = new List<Operacao.Business.Entities.Duplicata>();

            foreach (var dup in duplicatas)
            {
                lista.Add(new Duplicata(dup.Id, dup.Numero, dup.DataVencimento, dup.Valor));
            }

            return lista;
        }

        private void ValidarCheckout(Carrinho.Business.AggregateRoots.Carrinho carrinho)
        {
            if (carrinho.EstaProcessando())
            {
                _notificador.Notificar("Carrinho já está em processamento");
                return;
            }

            if (!carrinho.Duplicatas.Any())
            {
                _notificador.Notificar("Carrinho sem duplicatas");
                return;
            }
        }

        private bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        private void ValidarDuplicatas(List<string> duplicatasIds)
        {
            var duplicatas = _duplicataRepository.ListarPorIds(duplicatasIds);
            if (duplicatas == null || !duplicatas.Any())
            {
                _notificador.Notificar("Nenhuma duplicata encontrada para os IDs fornecidos");
                return;
            }

            if (duplicatas.Count() != duplicatasIds.Count)
            {
                _notificador.Notificar("Algumas duplicatas não foram encontradas para os IDs fornecidos");
                return;
            }

            if (duplicatas.Any(d => d.NoCarrinho))
            {
                _notificador.Notificar("Algumas duplicatas já estão no carrinho");
                return;
            }

            if (duplicatas.Any(d => d.Status != Core.Enums.EDuplicataStatus.DISPONIVEL))
            {
                _notificador.Notificar("Algumas duplicatas não estão disponíveis para adição ao carrinho");
                return;
            }

            if (duplicatas.Any(d => d.DataVencimento < DateTime.Now))
            {
                _notificador.Notificar("Algumas duplicatas estão vencidas e não podem ser adicionadas ao carrinho");
                return;
            }

        }



        #endregion
    }
}
