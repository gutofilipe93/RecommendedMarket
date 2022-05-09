using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.UI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetToken(TokebDto tokenDto)
        {
            try
            {
                var token = await _tokenService.GetTokenAsync(tokenDto.Email, tokenDto.Password);
                return Ok(token);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("reflesh")]
        public async Task<IActionResult> RefleshTokenAsync(TokebDto tokenDto)
        {
            try
            {                
                var token = await _tokenService.RefleshTokenAsync(tokenDto.RefleshToken);
                return Ok(token);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
