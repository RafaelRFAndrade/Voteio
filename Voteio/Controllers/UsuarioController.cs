using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Voteio.Interfaces.Repository;
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
            catch (Exception ex)
            {
                _logger.LogError($"deu ruim kk {ex.Message}");
                throw;
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var usuario = _usuarioService.ValidarLogin(loginRequest);

            var token = _tokenService.GenerateToken(usuario);

            return Ok(new { Token = token });
        }
    }
}
