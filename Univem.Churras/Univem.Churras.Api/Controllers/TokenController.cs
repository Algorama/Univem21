using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Helpers;
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
    public class TokenController : ControllerBase
    {
        private readonly ColegaService _colegaService;
        private readonly ITokenHelper _tokenHelper;

        public TokenController(ColegaService colegaService, ITokenHelper tokenHelper)
        {
            _colegaService = colegaService;
            _tokenHelper = tokenHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _colegaService.Login(request);
                if (token == null)
                    return Unauthorized();

                var tokenString = _tokenHelper.GetTokenString(token);
                if(string.IsNullOrWhiteSpace(tokenString))
                    return Unauthorized();

                return Ok(tokenString);
            }
            catch (ValidatorException ex)
            {
                return Unauthorized(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
