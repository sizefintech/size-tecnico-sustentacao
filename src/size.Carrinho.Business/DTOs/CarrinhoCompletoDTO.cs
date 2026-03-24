using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace size.Carrinho.Business.DTOs
{
    public class CarrinhoCompletoDTO
    {
        [JsonProperty("tomador_id")]
        public string TomadorId { get; set; }
        [JsonProperty("inicio_processamento")]
        public DateTime? InicioProcessamento { get; set; }
        [JsonProperty("valor")]
        public decimal Valor { get; set; }
        [JsonProperty("duplicatas")]
        public IEnumerable<DuplicataCarrinhoDTO> Duplicatas { get; set; } = new List<DuplicataCarrinhoDTO>();


    }

    public class DuplicataCarrinhoDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("valor")]
        public decimal Valor { get; set; }
        [JsonProperty("vencimento")]
        public DateTime Vencimento { get; set; }
    }
}
