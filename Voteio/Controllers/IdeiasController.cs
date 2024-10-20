using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Messaging.Responses;
using Voteio.Messaging.Responses.Base;
using Voteio.Services;

namespace Voteio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdeiasController : ControllerBase
    {
        private readonly IdeiasService _ideiasService;
        private readonly ILogger<IdeiasController> _logger;
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public IdeiasController(
            IdeiasService ideasService,
            ILogger<IdeiasController> logger,
            UsuarioService usuarioService,
            TokenService tokenService)
        {
            _ideiasService = ideasService;
            _logger = logger;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public ActionResult<ListarIdeiasResponse> ObterIdeias()
        {
            try
            {
                return _ideiasService.ObterIdeias();
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
        [HttpPost]
        public ActionResult<ResponseBase> CriarIdeia(RegistrarIdeiaRequest registrarIdeiaRequest)
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioService.ObterPorId(usuarioId);

                _ideiasService.RegistrarIdeia(registrarIdeiaRequest, usuario);

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

        [Authorize]
        [HttpPost("Comentario")]
        public ActionResult<ResponseBase> CriarComentario(RegistrarComentarioRequest registrarComentarioRequest)
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioService.ObterPorId(usuarioId);

                _ideiasService.RegistrarComentario(registrarComentarioRequest, usuario);

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

        [Authorize]
        [HttpPost("Vote")]
        public ActionResult<ResponseBase> AvaliarIdeia(AvaliarIdeiaRequest avaliarIdeiaRequest)
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var usuario = _usuarioService.ObterPorId(usuarioId);

                _ideiasService.AvaliarIdeia(avaliarIdeiaRequest, usuario);

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
    }
}
