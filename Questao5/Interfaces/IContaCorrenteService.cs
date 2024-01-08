using Questao5.Domain.Entities;
using Questao5.Domain.Models;

namespace Questao5.Interfaces
{
    public interface IContaCorrenteService
    {
        List<ContaCorrente> GetContasCorrente();
        Movimentado VerificarContasCorrente(Movimentar movimentar);
        MovimentoContaCorrente SaldoContacorrente(string identificacao);
    }
}
