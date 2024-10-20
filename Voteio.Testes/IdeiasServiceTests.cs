using Moq;
using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Services;
using Voteio.Messaging.Enums;
using Voteio.Messaging.RawQuery;

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
        public void RegistrarIdeia_QuandoUsuarioENull()
        {
            // Arrange
            var registrarRequest = new RegistrarIdeiaRequest { Titulo = "Ideia Teste", Descricao = "Descricao Teste" };
            Usuario? usuario = null;

            // Act & Assert
            var exception = Assert.Throws<VoteioException>(() => _ideiasService.RegistrarIdeia(registrarRequest, usuario));
            Assert.Equal("Usuário não identificado", exception.Message);
        }

        [Fact]
        public void RegistrarIdeia_ChamaRepo_QuandoUsuarioEValido()
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
        public void RegistrarComentario_DaException_QuandoUsuarioENull()
        {
            // Arrange
            var registrarRequest = new RegistrarComentarioRequest { CodigoIdeia = Guid.NewGuid(), Texto = "Comentario Teste" };
            Usuario? usuario = null;

            // Act & Assert
            var exception = Assert.Throws<VoteioException>(() => _ideiasService.RegistrarComentario(registrarRequest, usuario));
            Assert.Equal("Usuário não identificado", exception.Message);
        }

        [Fact]
        public void RegistrarComentario_ChamaRepo_QuandoUsuarioEValido()
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
        public void AvaliarIdeia_DaException_QuandoUsuarioENull()
        {
            // Arrange
            var avaliarRequest = new AvaliarIdeiaRequest { CodigoIdeia = Guid.NewGuid(), TipoVote = TipoVote.Upvote };
            Usuario? usuario = null;

            // Act & Assert
            var exception = Assert.Throws<VoteioException>(() => _ideiasService.AvaliarIdeia(avaliarRequest, usuario));
            Assert.Equal("Usuário não identificado", exception.Message);
        }

        [Fact]
        public void AvaliarIdeia_ChamaRepo_QuandoUsuarioEValido()
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
        public void AvaliarIdeia_DaException_QuandoUsuarioJaVotou()
        {
            // Arrange
            var avaliarRequest = new AvaliarIdeiaRequest { CodigoIdeia = Guid.NewGuid(), TipoVote = TipoVote.Upvote };
            var usuario = new Usuario { Codigo = Guid.NewGuid() };

            _votesRepositoryMock.Setup(repo => repo.ValidarSeJaFoiVotadoPorUsuario(usuario.Codigo, avaliarRequest.CodigoIdeia))
                .Returns(new CountRawQuery { Count = 1 });

            // Act & Assert
            var exception = Assert.Throws<VoteioException>(() => _ideiasService.AvaliarIdeia(avaliarRequest, usuario));
            Assert.Equal("Não é possível votar mais uma vez", exception.Message);
        }
    }
}