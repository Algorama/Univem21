using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Empresa.Churras.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColegaController : ControllerBase
    {
        private readonly ColegaService _service;

        public ColegaController(ColegaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Colega>> Get() => await _service.List();

        [HttpGet("{key}")]
        public async Task<Colega> Get(long key) => await _service.Get(key);
    }
}
