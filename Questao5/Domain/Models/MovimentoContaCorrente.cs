using System.Text.Json.Serialization;

namespace Questao5.Domain.Models
{
    public class MovimentoContaCorrente
    {
        public string resultado { get; set; }
        public string saldo { get; set; }
        public string numero { get; set; }
        public string titular { get; set; }
        public string dataHoraConsulta { get; set; }

        [JsonIgnore]
        public bool exists { get; set; }
    }
}
