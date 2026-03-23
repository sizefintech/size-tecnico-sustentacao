using Newtonsoft.Json;

namespace size.Core.DTOs
{
    public class DuplicataOperacaoDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("numero")]
        public string Numero { get; set; }
        [JsonProperty("vencimento")]
        public DateTime Vencimento { get; set; }
        [JsonProperty("valor")]
        public decimal Valor { get; set; }
        [JsonProperty("operacao_id")]
        public string OperacaoId { get; set; }
    }
}
