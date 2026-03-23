using Microsoft.AspNetCore.Mvc;
using size.Core.DTOs;
using size.Operacao.Application.Services;
using size.Operacao.Business.AggregateRoots;
using size.Operacao.Business.Entities;
using size_antecipacao.DTOs;

namespace size_antecipacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacaoController : ControllerBase
    {
        private readonly OperacaoService _operacaoService;

        public OperacaoController(OperacaoService operacaoService)
        {
            _operacaoService = operacaoService;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var operacao = await _operacaoService.ObterPorId(id);
            if (operacao == null)
                return NotFound();
            return Ok(operacao);
        }

        [HttpGet("resumo/{id}")]
        public async Task<IActionResult> ObterResumoPorId(string id)
        {
            var operacao = await _operacaoService.ObterPorId(id);
            if (operacao == null)
                return NotFound();

            return Ok(ToOperacaoDTO(operacao));
        }



        private OperacaoDTO ToOperacaoDTO(Operacao operacao)
        {
            return new OperacaoDTO()
            {
                Id = operacao.Id,
                AtualizadoEm = operacao.AtualizadoEm,
                Codigo = operacao.Codigo,
                CriadoEm = operacao.CriadoEm,
                DataCriacao = operacao.DataCriacao,
                DataProcessamento = operacao.DataProcessamento,
                Prazo = operacao.Prazo,
                TaxaAntecipacao = operacao.TaxaAntecipacao,
                TomadorId = operacao.TomadorId,
                ValorBruto = operacao.ValorBruto,
                ValorLiquido = operacao.ValorLiquido,
                Duplicatas = ToDuplicatasOperacaoDTO(operacao.Duplicatas.ToList())
            };
        }

        private List<DuplicataOperacaoDTO> ToDuplicatasOperacaoDTO(List<Duplicata> duplicatas)
        {
            var lista = new List<DuplicataOperacaoDTO>();
            if (duplicatas == null || !duplicatas.Any()) return lista;

            duplicatas.ForEach(x => lista.Add(ToDuplicataOperacaoDTO(x)));

            return lista;
        }

        private DuplicataOperacaoDTO ToDuplicataOperacaoDTO(Duplicata duplicata)
        {
            return new DuplicataOperacaoDTO()
            {
                Id = duplicata.Id,
                Numero = duplicata.Numero,
                OperacaoId = duplicata.OperacaoId,
                Valor = duplicata.Valor,
                Vencimento = duplicata.Vencimento
            };
        }
    }
}
