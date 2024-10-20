using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Messaging.Responses.Base;
using Voteio.Services;

namespace Voteio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public UsuarioController(ILogger<UsuarioController> logger,
            UsuarioService usuarioService,
            TokenService tokenService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public ActionResult<ResponseBase> Cadastrar(CadastrarUsuarioRequest request)
        {
            try
            {
                _usuarioService.Cadastrar(request);

                return new ResponseBase();
            }
            catch (VoteioException ex)
            {
                return BadRequest(new { Sucesso = false, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Sucesso = false, Mensagem = "Ocorreu um erro de requisição." });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var usuario = _usuarioService.ValidarLogin(loginRequest);

                var token = _tokenService.GenerateToken(usuario);

                return Ok(new { Token = token });
            }
            catch (VoteioException ex)
            {
                return BadRequest(new { Sucesso = false, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Sucesso = false, Mensagem = "Ocorreu um erro de requisição." });
            }
        }

        [Authorize]
        [HttpGet("ObterIdeiasVotadas")]
        public IActionResult ObterIdeiasVotadasPorUsuario()
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioService.ObterPorId(usuarioId);

                var ideiasVotadas = _usuarioService.ObterIdeiasVotadas(usuario.Codigo);

                return Ok( new { IdeiasVotadas = ideiasVotadas });
            }
            catch (VoteioException ex)
            {
                return BadRequest(new { Sucesso = false, Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Sucesso = false, Mensagem = "Ocorreu um erro de requisição." });
            }
        }
    }
}
