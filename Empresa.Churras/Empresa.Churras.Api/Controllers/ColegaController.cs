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
    public class ColegaController : ControllerBase
    {
        private readonly ColegaService _service;
        private readonly EventoService _eventoService;

        public ColegaController(ColegaService service, EventoService eventoService)
        {
            _service = service;
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IEnumerable<Colega>> Get() => await _service.List();

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
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{key}/eventos")]
        public async Task<IEnumerable<Evento>> GetEventos(long key)
        {
            return await _eventoService.List(x => x.DonoDaCasaKey == key);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Colega colega)
        {
            try
            {
                await _service.Insert(colega);

                return CreatedAtAction(nameof(Get), new { key = colega.Key }, colega);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Colega colega)
        {
            try
            {
                var entity = await _service.Get(colega.Key);
                if (entity == null)
                    return NotFound();

                await _service.Update(colega);

                return Ok(colega);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
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
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
