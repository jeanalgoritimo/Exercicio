using Questao5.Domain.Entities;
using System.Text.Json.Serialization;

namespace Questao5.Domain.Models
{
    public class Movimentado
    {
        public string resultado { get; set; }
        [JsonIgnore]
        public bool movimentar { get; set; }
        public Movimento movimento { get; set; }

    }
}
