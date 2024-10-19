using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Enums;
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

        public void Cadastrar(CadastrarUsuarioRequest request)
        {
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
                throw new Exception("Usuário não existe.");

            var passwordHasher = new PasswordHasher<Usuario>();

            var verificarSenha = passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, loginRequest.Password);

            if (verificarSenha == PasswordVerificationResult.Failed)
                throw new Exception("Senha inválida.");

            return usuario;
        }
    }
}
