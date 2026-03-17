using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace size.Core.DomainObjects
{
    public class ErrorDto
    {
        [Display(Description = "Lista de erros")]
        [JsonPropertyName("erros")]
        [JsonProperty("erros")]
        public List<string> Erros { get; set; } = new List<string>();

        public void AdicionarErro(string erro)
        {
            if (string.IsNullOrEmpty(erro?.Trim()))
                return;

            Erros.Add(erro);
        }
    }
}
