using Questao5.Domain.Entities;
using Questao5.Domain.Models;
using Questao5.Infrastructure.Sqlite;
using Questao5.Interfaces;

namespace Questao5.Services
{
    public class ContaCorrenteService: IContaCorrenteService
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;
        public ContaCorrenteService(IDatabaseBootstrap databaseBootstrap)
        {
            this._databaseBootstrap = databaseBootstrap;

        }

        public List<ContaCorrente> GetContasCorrente()
        {
             return this._databaseBootstrap.GetContasCorente();
        }

        public Movimentado VerificarContasCorrente(Movimentar movimentar)
        {
            return this._databaseBootstrap.VerificarContasCorrente(movimentar);  
        }
        public MovimentoContaCorrente SaldoContacorrente(string identificacao)
        {
            return this._databaseBootstrap.SaldoContacorrente(identificacao);
        }
    }
}
