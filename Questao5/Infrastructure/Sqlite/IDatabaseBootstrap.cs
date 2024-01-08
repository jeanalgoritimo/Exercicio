using Questao5.Domain.Entities;
using Questao5.Domain.Models;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();
        List<ContaCorrente> GetContasCorente();
        Movimentado VerificarContasCorrente(Movimentar movimentar);
        MovimentoContaCorrente SaldoContacorrente(string identificacao);
    }
}

