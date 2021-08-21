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
    public class EventoController : ControllerBase
    {
        private readonly EventoService _eventoService;

        public EventoController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IEnumerable<Evento>> GetList()
        {
            var eventos = await _eventoService.List();
            return eventos;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(long key)
        {
            try
            {
                var evento = await _eventoService.Get(key);
                if (evento == null)
                    return NotFound();

                return Ok(evento);
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
        public async Task<IActionResult> Post([FromBody] Evento evento)
        {
            try
            {
                await _eventoService.Insert(evento);
                return CreatedAtAction(nameof(Get), new { key = evento.Key }, evento);
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
        public async Task<IActionResult> Put([FromBody] Evento evento)
        {
            try
            {
                var fromDb = await _eventoService.Get(evento.Key);
                if (fromDb == null)
                    return NotFound();

                await _eventoService.Update(evento);
                return Ok(evento);
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
                var evento = await _eventoService.Get(key);
                if (evento == null)
                    return NotFound();

                await _eventoService.Delete(evento);
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

        [HttpPatch("{key}/confirmarPresenca/{vouLevar}")]
        public async Task<IActionResult> PatchConfirmarPresenca(long key, string vouLevar)
        {
            try
            {
                var evento = await _eventoService.Get(key);
                if (evento == null)
                    return NotFound();

                await _eventoService.ConfirmarPresenca(key, vouLevar);

                evento = await _eventoService.Get(key);

                return Ok(evento);
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

        [HttpPatch("{key}/cancelarPresenca")]
        public async Task<IActionResult> PatchCancelarPresenca(long key)
        {
            try
            {
                var evento = await _eventoService.Get(key);
                if (evento == null)
                    return NotFound();

                await _eventoService.CancelarPresenca(key);

                evento = await _eventoService.Get(key);

                return Ok(evento);
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
