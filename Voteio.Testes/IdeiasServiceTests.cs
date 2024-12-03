using Moq;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Services;
using Voteio.Messaging.Enums;
using Voteio.Messaging.RawQuery;
using FluentAssertions;

namespace Voteio.Testes
{
    public class IdeiasServiceTests
    {
        private readonly Mock<IIdeiasRepository> _ideiasRepositoryMock;
        private readonly Mock<IComentarioRepository> _comentarioRepositoryMock;
        private readonly Mock<IVotesRepository> _votesRepositoryMock;
        private readonly IdeiasService _ideiasService;

        public IdeiasServiceTests()
        {
            _ideiasRepositoryMock = new Mock<IIdeiasRepository>();
            _comentarioRepositoryMock = new Mock<IComentarioRepository>();
            _votesRepositoryMock = new Mock<IVotesRepository>();
            _ideiasService = new IdeiasService(
                _ideiasRepositoryMock.Object,
                _comentarioRepositoryMock.Object,
                _votesRepositoryMock.Object);
        }

        [Fact]
        public void RegistrarIdeia_ShouldThrowException_WhenUsuarioIsNull()
        {
            // Arrange
            var registrarRequest = new RegistrarIdeiaRequest { Titulo = "Ideia Teste", Descricao = "Descricao Teste" };
            Usuario? usuario = null;

            // Act
            Action act = () => _ideiasService.RegistrarIdeia(registrarRequest, usuario);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário não identificado");
        }

        [Fact]
        public void RegistrarIdeia_ShouldCallRepository_WhenUsuarioIsValid()
        {
            // Arrange
            var registrarRequest = new RegistrarIdeiaRequest { Titulo = "Ideia Teste", Descricao = "Descricao Teste" };
            var usuario = new Usuario { Codigo = Guid.NewGuid() };

            // Act
            _ideiasService.RegistrarIdeia(registrarRequest, usuario);

            // Assert
            _ideiasRepositoryMock.Verify(repo => repo.InserirIdeia(It.IsAny<Ideias>()), Times.Once);
        }

        [Fact]
        public void RegistrarComentario_ShouldThrowException_WhenUsuarioIsNull()
        {
            // Arrange
            var registrarRequest = new RegistrarComentarioRequest { CodigoIdeia = Guid.NewGuid(), Texto = "Comentario Teste" };
            Usuario? usuario = null;

            // Act
            Action act = () => _ideiasService.RegistrarComentario(registrarRequest, usuario);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário não identificado");
        }

        [Fact]
        public void RegistrarComentario_ShouldCallRepository_WhenUsuarioIsValid()
        {
            // Arrange
            var registrarRequest = new RegistrarComentarioRequest { CodigoIdeia = Guid.NewGuid(), Texto = "Comentario Teste" };
            var usuario = new Usuario { Codigo = Guid.NewGuid() };

            // Act
            _ideiasService.RegistrarComentario(registrarRequest, usuario);

            // Assert
            _comentarioRepositoryMock.Verify(repo => repo.InserirComentario(It.IsAny<Comentario>()), Times.Once);
        }

        [Fact]
        public void AvaliarIdeia_ShouldThrowException_WhenUsuarioIsNull()
        {
            // Arrange
            var avaliarRequest = new AvaliarIdeiaRequest { CodigoIdeia = Guid.NewGuid(), TipoVote = TipoVote.Upvote };
            Usuario? usuario = null;

            // Act
            Action act = () => _ideiasService.AvaliarIdeia(avaliarRequest, usuario);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Usuário não identificado");
        }

        [Fact]
        public void AvaliarIdeia_ShouldCallRepository_WhenUsuarioIsValid()
        {
            // Arrange
            var avaliarRequest = new AvaliarIdeiaRequest { CodigoIdeia = Guid.NewGuid(), TipoVote = TipoVote.Upvote };
            var usuario = new Usuario { Codigo = Guid.NewGuid() };

            _votesRepositoryMock.Setup(repo => repo.ValidarSeJaFoiVotadoPorUsuario(usuario.Codigo, avaliarRequest.CodigoIdeia))
                .Returns(new CountRawQuery { Count = 0 });

            // Act
            _ideiasService.AvaliarIdeia(avaliarRequest, usuario);

            // Assert
            _votesRepositoryMock.Verify(repo => repo.InserirAvaliacao(It.IsAny<Votes>()), Times.Once);
        }

        [Fact]
        public void AvaliarIdeia_ShouldThrowException_WhenUsuarioJaVotou()
        {
            // Arrange
            var avaliarRequest = new AvaliarIdeiaRequest { CodigoIdeia = Guid.NewGuid(), TipoVote = TipoVote.Upvote };
            var usuario = new Usuario { Codigo = Guid.NewGuid() };

            _votesRepositoryMock.Setup(repo => repo.ValidarSeJaFoiVotadoPorUsuario(usuario.Codigo, avaliarRequest.CodigoIdeia))
                .Returns(new CountRawQuery { Count = 1 });

            // Act
            Action act = () => _ideiasService.AvaliarIdeia(avaliarRequest, usuario);

            // Assert
            act.Should().Throw<VoteioException>().WithMessage("Não é possível votar mais uma vez");
        }
    }
}