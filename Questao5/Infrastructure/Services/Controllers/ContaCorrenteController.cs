using Microsoft.AspNetCore.Mvc;
using Questao5.Domain.Entities;
using Questao5.Domain.Models;
using Questao5.Interfaces;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteService _contaCorrenteService;
        public ContaCorrenteController(IContaCorrenteService contaCorrenteService)
        {
            this._contaCorrenteService = contaCorrenteService;

        }
        [HttpGet]
        [Route("GetContasCorrente")]
        public List<ContaCorrente> GetContasCorrente()
        {
            return this._contaCorrenteService.GetContasCorrente();
        }

        [HttpPost]
        [Route("MovimentarContasCorrente")]
        public ActionResult MovimentarContasCorrente([FromBody] Movimentar movimentar)
        {
            var movimento = this._contaCorrenteService.VerificarContasCorrente(movimentar);

            if(!movimento.movimentar)
            {
                return BadRequest(movimento);
            }
            else
            {
                return Ok(movimento);
            }
        }

        [HttpGet]
        [Route("SaldoContasCorrente")]
        public ActionResult SaldoContasCorrente([FromQuery] string identificacao)
        {
            var movimentoSaldo = this._contaCorrenteService.SaldoContacorrente(identificacao);
            if (!movimentoSaldo.exists)
            {
                return BadRequest(movimentoSaldo);
            }
            else
            {
               return Ok(movimentoSaldo);
            }
        }
    }
}
