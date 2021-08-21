using Kernel.Domain.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Services;

namespace Univem.Churras.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColegaController : ControllerBase
    {
        private readonly ColegaService _colegaService;
        private readonly EventoService _eventoService;

        public ColegaController(ColegaService colegaService, EventoService eventoService)
        {
            _colegaService = colegaService;
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IEnumerable<Colega>> GetList()
        {
            var colegas = await _colegaService.List();
            return colegas;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(long key)
        {
            try
            {
                var colega = await _colegaService.Get(key);
                if (colega == null)
                    return NotFound();

                return Ok(colega);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("{key}/eventos")]
        public async Task<IActionResult> GetEventos(long key)
        {
            try
            {
                var eventos = await _eventoService.List(x => x.DonoDaCasaKey == key);
                return Ok(eventos);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Colega colega)
        {
            try
            {
                await _colegaService.Insert(colega);
                return CreatedAtAction(nameof(Get), new { key = colega.Key }, colega);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Colega colega)
        {
            try
            {
                var fromDb = await _colegaService.Get(colega.Key);
                if (fromDb == null)
                    return NotFound();

                await _colegaService.Update(colega);
                return Ok(colega);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(long key)
        {
            try
            {
                var colega = await _colegaService.Get(key);
                if (colega == null)
                    return NotFound();

                await _colegaService.Delete(colega);
                return Ok();
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
