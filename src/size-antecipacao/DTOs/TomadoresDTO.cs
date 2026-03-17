using Newtonsoft.Json;
using size.Core.Enums;

namespace size_antecipacao.DTOs
{
    public class TomadoresDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nome")]
        public string Nome { get; set; }
        [JsonProperty("documento")]
        public string Documento { get; set; }

        public List<DuplicataDTO> Duplicatas { get; set; } = new List<DuplicataDTO>();
    }

    public class DuplicataDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("numero")]
        public string Numero { get; set; }
        [JsonProperty("valor")]
        public decimal Valor { get; set; }
        [JsonProperty("no_carrinho")]
        public bool NoCarrinho { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
