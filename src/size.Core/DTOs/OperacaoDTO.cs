using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.DTOs
{
    public class OperacaoDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tomador_id")]
        public string TomadorId { get; set; }
        [JsonProperty("codigo")]
        public string Codigo { get; set; }
        [JsonProperty("valor_bruto")]
        public decimal ValorBruto { get; set; }
        [JsonProperty("valor_liquido")]
        public decimal ValorLiquido { get; set; }
        [JsonProperty("taxa_antecipacao")]
        public decimal TaxaAntecipacao { get; set; }

        [JsonProperty("prazo")]
        public int Prazo { get; set; }

        [JsonProperty("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("data_processamento")]
        public DateTime? DataProcessamento { get; set; }

        [JsonProperty("criado_em")]
        public DateTime CriadoEm { get; set; }

        [JsonProperty("atualizado_em")]
        public DateTime AtualizadoEm { get; set; }

        [JsonProperty("duplicatas")]
        public List<DuplicataOperacaoDTO> Duplicatas { get; set; }

    }
}
