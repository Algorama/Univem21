using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Services;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<Evento> Get(long key) => await _service.Get(key);
    }
}
