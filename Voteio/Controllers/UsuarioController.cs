using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voteio.Entities;
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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public UsuarioController(ILogger<UsuarioController> logger,
            IUsuarioRepository usuarioRepository,
            UsuarioService usuarioService,
            TokenService tokenService)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        //endpoint teste
        [Authorize]
        [HttpGet]
        public ActionResult<List<Usuario>> GetUsuario()
        {
            //forma de pegar usuario via token
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuario = _usuarioService.ObterPorId(usuarioId);
            //

            var usuarios = _usuarioRepository.ListarUsuarios();

            return usuarios;
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
