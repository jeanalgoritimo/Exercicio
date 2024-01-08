using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Models;
using System;
using System.Globalization;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseBootstrap(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public void Setup()
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND (name = 'contacorrente' or name = 'movimento' or name = 'idempotencia');");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && (tableName == "contacorrente" || tableName == "movimento" || tableName == "idempotencia"))
                return;

            connection.Execute("CREATE TABLE contacorrente ( " +
                               "idcontacorrente TEXT(37) PRIMARY KEY," +
                               "numero INTEGER(10) NOT NULL UNIQUE," +
                               "nome TEXT(100) NOT NULL," +
                               "ativo INTEGER(1) NOT NULL default 0," +
                               "CHECK(ativo in (0, 1)) " +
                               ");");

            connection.Execute("CREATE TABLE movimento ( " +
                "idmovimento TEXT(37) PRIMARY KEY," +
                "idcontacorrente TEXT(37) NOT NULL," +
                "datamovimento TEXT(25) NOT NULL," +
                "tipomovimento TEXT(1) NOT NULL," +
                "valor REAL NOT NULL," +
                "CHECK(tipomovimento in ('C', 'D')), " +
                "FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente) " +
                ");");

            connection.Execute("CREATE TABLE idempotencia (" +
                               "chave_idempotencia TEXT(37) PRIMARY KEY," +
                               "requisicao TEXT(1000)," +
                               "resultado TEXT(1000));");

            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('B6BAFC09-6967-ED11-A567-055DFA4A16C9', 123, 'Katherine Sanchez', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('FA99D033-7067-ED11-96C6-7C5DFA4A16C9', 456, 'Eva Woodward', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('382D323D-7067-ED11-8866-7D5DFA4A16C9', 789, 'Tevin Mcconnell', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('F475F943-7067-ED11-A06B-7E5DFA4A16C9', 741, 'Ameena Lynn', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('BCDACA4A-7067-ED11-AF81-825DFA4A16C9', 852, 'Jarrad Mckee', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('D2E02051-7067-ED11-94C0-835DFA4A16C9', 963, 'Elisha Simons', 0);");
        }

        public List<ContaCorrente> GetContasCorente()
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var contaCorrente = connection.Query<ContaCorrente>("SELECT * FROM contacorrente;").ToList();

            return contaCorrente;
        }

        public Movimentado VerificarContasCorrente(Movimentar movimentar)
        {
            Movimentado movimento = new Movimentado();

            if ((movimentar.tipoMovimento != "c" && movimentar.tipoMovimento != "C")
                && (movimentar.tipoMovimento != "d" && movimentar.tipoMovimento != "D"))
            {
                movimento.resultado = "INVALID_TYPE";
                movimento.movimentar = false;
                return movimento;
            }
            movimentar.identificacao = "";
            using var connection = new SqliteConnection(databaseConfig.Name);

            var ListcontaCorrente = connection.Query<ContaCorrente>("SELECT * FROM contacorrente;").ToList();

            var contaCorrente = ListcontaCorrente.Where(x => x.idcontacorrente == movimentar.identificacao
                                                        || x.numero == Convert.ToInt32(movimentar.numero)).FirstOrDefault();

            if (contaCorrente.ativo == 1)
            {


                if (!String.IsNullOrEmpty(movimentar.valorMovimentado) && movimentar.valorMovimentado.Contains("-"))
                {
                    movimento.movimentar = false;
                    movimento.resultado = "INVALID_VALUE";

                }
                else
                {
                    movimento.movimento = new Movimento();
                    movimento.movimentar = true;
                    movimento.movimento.idcontacorrente = contaCorrente.idcontacorrente;
                    movimento.movimento.valor = Convert.ToDecimal(movimentar.valorMovimentado);
                    movimento.movimento.tipomovimento = movimentar.tipoMovimento;
                    movimentar.identificacao = contaCorrente.idcontacorrente;
                    movimento.movimento = Movimentar(movimentar);
                    movimento.resultado = "Sucesso"; 
                    return movimento;

                }
            }
            else if (contaCorrente.ativo == 0)
            {
                movimento.movimentar = false;
                movimento.resultado = "INACTIVE_ACCOUNT";
            }
            else if (contaCorrente == null)
            {
                movimento.movimentar = false;
                movimento.resultado = "INVALID_ACCOUNT";

            }
            return movimento;
        }
        private Movimento Movimentar(Movimentar movimentar)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);
            var chave = alfanumericoAleatorio(37);

            connection.Execute("INSERT INTO movimento(idmovimento,idcontacorrente,datamovimento,tipomovimento,valor)" +
                     " VALUES('" + chave + "','" + movimentar.identificacao + "','" + DateTime.Now.ToString() + "','" + movimentar.tipoMovimento.ToUpper() + "'," + movimentar.valorMovimentado + ");");

            var Listmovimento = connection.Query<Movimento>("SELECT * FROM movimento;").ToList();

            var movimento = Listmovimento.Where(x => x.idmovimento == chave).FirstOrDefault();
            return movimento;
        }

        public MovimentoContaCorrente SaldoContacorrente(string identificacao)
        {
            MovimentoContaCorrente movimentoContaCorrente = new MovimentoContaCorrente();
          
            using var connection = new SqliteConnection(databaseConfig.Name);
            var Listmovimento = connection.Query<Movimento>("SELECT * FROM movimento;").ToList();
            var ListcontaCorrente = connection.Query<ContaCorrente>("SELECT * FROM contacorrente;").ToList();

            var contaCorrente = ListcontaCorrente.Where(x => x.idcontacorrente == identificacao).FirstOrDefault();

            if (contaCorrente == null)
            {
                movimentoContaCorrente.resultado = "INVALID_ACCOUNT";
                movimentoContaCorrente.saldo = "0.0";
                movimentoContaCorrente.exists = false;
                return movimentoContaCorrente;
            }
            if (contaCorrente.ativo == 0)
            {
                movimentoContaCorrente.resultado = " INACTIVE_ACCOUNT";
                movimentoContaCorrente.saldo = "0.0";
                movimentoContaCorrente.exists = false;
                return movimentoContaCorrente;
            }

            var movimento = Listmovimento.Where(x => x.idcontacorrente == identificacao).ToList();


            if (movimento.Count > 0)
            {
                var listCredito = Listmovimento.Where(x => x.idcontacorrente == identificacao && x.tipomovimento == "C").ToList();
                var listDebito = Listmovimento.Where(x => x.idcontacorrente == identificacao && x.tipomovimento == "D").ToList();
                var saldoCredito = listCredito.Sum(item => item.valor);
                var saldoDebito = listDebito.Sum(item => item.valor);

                var saldoFinal = (saldoCredito - saldoDebito);
                movimentoContaCorrente.saldo = saldoFinal.ToString();
                movimentoContaCorrente.resultado = "Sucesso";
                movimentoContaCorrente.exists = true;
                movimentoContaCorrente.dataHoraConsulta = DateTime.Now.ToString();
                movimentoContaCorrente.numero = contaCorrente.numero.ToString();
                movimentoContaCorrente.titular = contaCorrente.nome;
            }

            return movimentoContaCorrente;
        }

        public static string alfanumericoAleatorio(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}
