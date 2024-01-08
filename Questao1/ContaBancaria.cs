using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Questao1
{
    public class ContaBancaria
    {
        public int Conta { get; set; }
        public string Titular { get; set; }
        public decimal ValorInicial { get; set; }
        public decimal ValorVerificado { get; set; }

        public double taxa = 3.50;
        
        public ContaBancaria(int conta, string Titular, decimal? ValorInicial)
        {
            this.Conta = conta;
            this.Titular = Titular;
            this.ValorInicial = (decimal)ValorInicial;
        }
        public void Deposito(decimal quantia)
        {
            this.ValorInicial = this.ValorInicial + quantia;
        }
        public void Saque(decimal quantia)
        {
            var valor = (this.ValorInicial - quantia);
            decimal retirado = Convert.ToDecimal(valor.ToString().Replace("-", ""));
            this.ValorInicial = retirado - Convert.ToDecimal(taxa);
        }
        public void VerificarSaque(decimal quantia)
        {
            var valor = (this.ValorInicial - quantia);
            decimal retirado = Convert.ToDecimal(valor.ToString().Replace("-", ""));
            this.ValorVerificado = retirado - Convert.ToDecimal(taxa);
        }
    }
}
