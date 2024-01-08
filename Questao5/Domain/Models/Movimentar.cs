using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using System.Text.Json.Serialization;

namespace Questao5.Domain.Models
{
    public class Movimentar
    {
    
        public string tipoMovimento { get; set; }
        public string identificacao { get; set; }
        public string numero { get; set; }
        public string valorMovimentado { get; set; }
    }

}
