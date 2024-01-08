using System;
using System.Globalization;

namespace Questao1
{
    class Program
    {
        static void Main(string[] args)
        {

            ContaBancaria conta;

            Console.Write("Entre o número da conta: ");
            int numero = int.Parse(Console.ReadLine());
            Console.Write("Entre o titular da conta: ");
            string titular = Console.ReadLine();
            Console.Write("Haverá depósito inicial (s/n)? ");
            char resp = char.Parse(Console.ReadLine());
            if (resp == 's' || resp == 'S')
            {
                Console.Write("Entre o valor de depósito inicial: ");
                decimal depositoInicial = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                conta = new ContaBancaria(numero, titular, depositoInicial);
            }
            else
            {
                conta = new ContaBancaria(numero, titular, 0);
            }

            Console.WriteLine();
            Console.WriteLine("Dados da conta:");
            Console.WriteLine("Conta: " + conta.Conta + ", Titular: " + conta.Titular + ", Saldo: $ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:D}", conta.ValorInicial.ToString().Replace(",", ".")));
             
            Console.WriteLine();
            Console.Write("Entre um valor para depósito: ");
            decimal quantia = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            conta.Deposito(quantia);
            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine("Conta: " + conta.Conta + ", Titular: " + conta.Titular + ", Saldo: $ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:D}", conta.ValorInicial.ToString().Replace(",", ".")));

            Console.WriteLine();
            Console.Write("Entre um valor para saque: ");
            quantia = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            conta.VerificarSaque(quantia);
            decimal verificado = 0;
            if (conta.ValorVerificado.ToString().Contains("-"))
            {
                verificado = Convert.ToDecimal(conta.ValorVerificado.ToString().Replace("-", ""));
                if (Convert.ToDecimal(conta.taxa) < verificado)
                {
                    Console.WriteLine("Saldo não é suficiente para realizar o saque ");
                }
                else
                {
                    conta.Saque(quantia);
                    Console.WriteLine("Dados da conta atualizados:");
                    Console.WriteLine("Conta: " + conta.Conta + ", Titular: " + conta.Titular + ", Saldo: $ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:D}", conta.ValorInicial.ToString().Replace(",", ".")));
                }
            }
            else
            {
                conta.Saque(quantia);
                Console.WriteLine("Dados da conta atualizados:");
                Console.WriteLine("Conta: " + conta.Conta + ", Titular: " + conta.Titular + ", Saldo: $ " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:D}", conta.ValorInicial.ToString().Replace(",", ".")));
            }
            

          


        }
    }
}
