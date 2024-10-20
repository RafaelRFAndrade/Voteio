using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Enums;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.RawQuery;
using Voteio.Messaging.Requests;

namespace Voteio.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario ObterPorId(string id)
        {
            return _usuarioRepository.ObterPorId(id);
        }

        public void Cadastrar(CadastrarUsuarioRequest request)
        {
            var validarExistenciaUsuario = _usuarioRepository.ObterPorEmail(request.Email);

            if (validarExistenciaUsuario is not null)
                throw new VoteioException("Usuário já cadastrado com esse email.");

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha, 
                Codigo = Guid.NewGuid(),
                DtInclusao = DateTime.Now,
                DtAtualizacao = DateTime.Now,
                Situacao = Situacao.Ativo
            };

            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.Senha = passwordHasher.HashPassword(usuario, request.Senha);

            _usuarioRepository.InserirUsuario(usuario);
        }

        public Usuario ValidarLogin(LoginRequest loginRequest)
        {
            var usuario = _usuarioRepository.ObterPorEmail(loginRequest.Email);

            if (usuario is null)
                throw new VoteioException("Usuário não existe.");

            var passwordHasher = new PasswordHasher<Usuario>();

            var verificarSenha = passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, loginRequest.Password);

            if (verificarSenha == PasswordVerificationResult.Failed)
                throw new VoteioException("Senha inválida.");

            return usuario;
        }

        public List<ObterIdeiasVotadasRawQuery> ObterIdeiasVotadas(Guid codigoUsuario)
        {
            return _usuarioRepository.ObterIdeiasVotadas(codigoUsuario);
        }
    }
}
