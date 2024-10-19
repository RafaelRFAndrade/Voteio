using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Requests;
using Voteio.Messaging.Responses.Base;
using Voteio.Repository.Base;
using Voteio.Services;

namespace Voteio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioService _usuarioService;

        public UsuariosController(ILogger<UsuariosController> logger,
            IUsuarioRepository usuarioRepository,
            UsuarioService usuarioService)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;   
        }

        [HttpGet]
        public ActionResult<List<Usuario>> GetUsuario()
        {
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
    }
}
