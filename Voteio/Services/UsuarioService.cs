using Microsoft.AspNetCore.Identity;
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
    }
}
