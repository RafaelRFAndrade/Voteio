using FluentAssertions;
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
        public void ObterPorId_ShouldReturnUsuario_WhenIdExists()
        {
            // Arrange
            var usuarioId = "123";
            var expectedUsuario = new Usuario { Nome = "Teste", Email = "teste@email.com", Codigo = Guid.NewGuid() };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuarioId)).Returns(expectedUsuario);

            // Act
            var obtainedUsuario = _usuarioService.ObterPorId(usuarioId);

            // Assert
            obtainedUsuario.Should().BeEquivalentTo(expectedUsuario);
        }

        [Fact]
        public void ObterPorId_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            var usuarioId = "nonexistent-id";
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(usuarioId)).Returns((Usuario)null);

            // Act
            var result = _usuarioService.ObterPorId(usuarioId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Cadastrar_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var request = new CadastrarUsuarioRequest { Nome = "Teste", Email = "teste@email.com", Senha = "senha123" };
            var existingUsuario = new Usuario { Email = request.Email };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(request.Email)).Returns(existingUsuario);

            // Act
            Action act = () => _usuarioService.Cadastrar(request);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário já cadastrado com esse email.");
        }

        [Fact]
        public void Cadastrar_ShouldInsertUsuario_WhenEmailDoesNotExist()
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
        public void ValidarLogin_ShouldThrowException_WhenUsuarioDoesNotExist()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "naoexiste@email.com", Password = "senha123" };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(loginRequest.Email)).Returns((Usuario)null);

            // Act
            Action act = () => _usuarioService.ValidarLogin(loginRequest);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário não existe.");
        }

        [Fact]
        public void ValidarLogin_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "teste@email.com", Password = "wrongpassword" };
            var usuario = new Usuario { Email = loginRequest.Email, Senha = "correctpassword" };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(loginRequest.Email)).Returns(usuario);

            // Act
            Action act = () => _usuarioService.ValidarLogin(loginRequest);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Senha incorreta.");
        }

        [Fact]
        public void ValidarLogin_ShouldReturnUsuario_WhenCredentialsAreCorrect()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "teste@email.com", Password = "senha123" };
            var expectedUsuario = new Usuario { Email = loginRequest.Email, Senha = "senha123" };

            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(loginRequest.Email)).Returns(expectedUsuario);

            // Act
            var result = _usuarioService.ValidarLogin(loginRequest);

            // Assert
            result.Should().BeEquivalentTo(expectedUsuario);
        }
    }
}