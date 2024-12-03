using FluentAssertions;
using Moq;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Services;
using Voteio.Testes.Base;

namespace Voteio.Testes
{
    public class UsuarioServiceTests : TestBase
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-id")]
        public void ObterPorId_ShouldReturnNull_WhenIdIsInvalid(string invalidId)
        {
            // Arrange
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorId(invalidId)).Returns((Usuario)null);

            // Act
            var result = _usuarioService.ObterPorId(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Cadastrar_ShouldValidateEmail_WhenEmailIsInvalid()
        {
            // Arrange
            var request = CreateTestUserRequest();
            request.Email = "invalid-email";

            // Act
            Action act = () => _usuarioService.Cadastrar(request);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Email inválido");
        }

        [Fact]
        public void ValidarLogin_ShouldHashPassword_WhenCreatingNewUser()
        {
            // Arrange
            var request = CreateTestUserRequest();
            _usuarioRepositoryMock.Setup(repo => repo.ObterPorEmail(request.Email)).Returns((Usuario)null);

            // Act
            _usuarioService.Cadastrar(request);

            // Assert
            _usuarioRepositoryMock.Verify(repo => 
                repo.InserirUsuario(It.Is<Usuario>(u => u.Senha != request.Senha)), Times.Once);
        }
    }
}