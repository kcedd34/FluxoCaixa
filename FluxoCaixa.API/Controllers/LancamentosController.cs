using FluxoCaixa.Application.DTOs;
using FluxoCaixa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentosController : ControllerBase
    {
        private readonly ILancamentoService _service;
        private readonly ILogger<LancamentosController> _logger;

        public LancamentosController(ILancamentoService service, ILogger<LancamentosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CriarLancamento([FromBody] LancamentoDto lancamento)
        {
            _logger.LogInformation("Criando novo lançamento");
            var id = await _service.CriarLancamentoAsync(lancamento);
            return CreatedAtAction(nameof(ObterLancamentoPorId), new { id }, id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterLancamentoPorId(Guid id)
        {
            _logger.LogInformation("Obtendo lançamento {Id}", id);
            var lancamento = await _service.ObterLancamentoPorIdAsync(id);

            if (lancamento == null)
                return NotFound();

            return Ok(lancamento);
        }

        [HttpGet("data/{data}")]
        public async Task<IActionResult> ObterLancamentosPorData(DateTime data)
        {
            _logger.LogInformation("Obtendo lançamentos da data {Data}", data);
            var lancamentos = await _service.ObterLancamentosPorDataAsync(data);
            return Ok(lancamentos);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> ObterLancamentosPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            _logger.LogInformation("Obtendo lançamentos do período {Inicio} a {Fim}", inicio, fim);
            var lancamentos = await _service.ObterLancamentosPorPeriodoAsync(inicio, fim);
            return Ok(lancamentos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLancamento(Guid id, [FromBody] LancamentoDto lancamento)
        {
            _logger.LogInformation("Atualizando lançamento {Id}", id);
            var resultado = await _service.AtualizarLancamentoAsync(id, lancamento);

            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverLancamento(Guid id)
        {
            _logger.LogInformation("Removendo lançamento {Id}", id);
            var resultado = await _service.RemoverLancamentoAsync(id);

            if (!resultado)
                return NotFound();

            return NoContent();
        }
    }
}