using FluentAssertions;
using Microsoft.AspNetCore.Identity.Data;
using Moq;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Services;

namespace Voteio.Testes
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public void ObterPorId_DeveRetornarUsuario_QuandoIdExistir()
        {
            // Arrange
            var usuarioId = "123";
            var usuarioEsperado = new Usuario { Nome = "Teste", Email = "teste@email.com", Codigo = Guid.NewGuid() };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuarioId)).Returns(usuarioEsperado);

            // Act
            var usuarioObtido = _usuarioService.ObterPorId(usuarioId);

            // Assert
            usuarioObtido.Should().BeEquivalentTo(usuarioEsperado);
        }

        [Fact]
        public void Cadastrar_DeveLancarExcecao_QuandoEmailJaExistir()
        {
            // Arrange
            var request = new CadastrarUsuarioRequest { Nome = "Teste", Email = "teste@email.com", Senha = "senha123" };
            var usuarioExistente = new Usuario { Email = request.Email };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(request.Email)).Returns(usuarioExistente);

            // Act
            Action act = () => _usuarioService.Cadastrar(request);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário já cadastrado com esse email.");
        }

        [Fact]
        public void Cadastrar_DeveInserirUsuario_QuandoEmailNaoExistir()
        {
            // Arrange
            var request = new CadastrarUsuarioRequest { Nome = "Teste", Email = "teste@email.com", Senha = "senha123" };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(request.Email)).Returns((Usuario)null);

            // Act
            _usuarioService.Cadastrar(request);

            // Assert
            _usuarioRepositoryMock.Verify(repo => repo.InserirUsuario(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void ValidarLogin_DeveLancarExcecao_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "naoexiste@email.com", Password = "senha123" };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(loginRequest.Email)).Returns((Usuario)null);

            // Act
            Action act = () => _usuarioService.ValidarLogin(loginRequest);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário não existe.");
        }
    }
}