using FluxoCaixa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsolidadoController : ControllerBase
    {
        private readonly IConsolidadoService _service;
        private readonly ILogger<ConsolidadoController> _logger;

        public ConsolidadoController(IConsolidadoService service, ILogger<ConsolidadoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("data/{data}")]
        public async Task<IActionResult> ObterConsolidadoPorData(DateTime data)
        {
            _logger.LogInformation("Obtendo consolidado da data {Data}", data);
            var consolidado = await _service.ObterConsolidadoPorDataAsync(data);
            return Ok(consolidado);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> ObterConsolidadoPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            _logger.LogInformation("Obtendo consolidados do período {Inicio} a {Fim}", inicio, fim);
            var consolidados = await _service.ObterConsolidadoPeriodoAsync(inicio, fim);
            return Ok(consolidados);
        }
    }
}