using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Services;
using Kernel.Domain.Model.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Empresa.Churras.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly EventoService _service;

        public EventoController(EventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Evento>> Get() => await _service.List();

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(long key)
        {
            try
            {
                var entity = await _service.Get(key);
                if (entity == null)
                    return NotFound();

                return Ok(entity);
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
                await _service.Insert(evento);

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
                var entity = await _service.Get(evento.Key);
                if (entity == null)
                    return NotFound();

                await _service.Update(evento);

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
                var entity = await _service.Get(key);
                if (entity == null)
                    return NotFound();

                await _service.Delete(entity);
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

        [HttpPut("{key}/confirmarPresenca/{vouLevar}")]
        public async Task<IActionResult> PutConfirmarPresenca(long key, string vouLevar)
        {
            try
            {
                var entity = await _service.Get(key);
                if (entity == null)
                    return NotFound();

                await _service.ConfirmarPresenca(key, vouLevar);

                entity = await _service.Get(key);

                return Ok(entity);
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

        [HttpPut("{key}/cancelarPresenca")]
        public async Task<IActionResult> PutCancelarPresenca(long key)
        {
            try
            {
                var entity = await _service.Get(key);
                if (entity == null)
                    return NotFound();

                await _service.CancelarPresenca(key);

                entity = await _service.Get(key);

                return Ok(entity);
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
