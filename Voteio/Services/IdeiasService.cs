using Voteio.Entities;
using Voteio.Interfaces.Repository;
using Voteio.Messaging.Exceptions;
using Voteio.Messaging.Requests;
using Voteio.Messaging.Responses;

namespace Voteio.Services
{
    public class IdeiasService
    {
        private readonly IIdeiasRepository _ideiasRepository;
        private readonly IComentarioRepository _comentarioRepository;
        private readonly IVotesRepository _votesRepository;

        public IdeiasService(
            IIdeiasRepository ideiasRepository,
            IComentarioRepository comentarioRepository,
            IVotesRepository votesRepository)
        {
            _ideiasRepository = ideiasRepository;
            _comentarioRepository = comentarioRepository;
            _votesRepository = votesRepository;
        }

        public ListarIdeiasResponse ObterIdeias(ObterIdeiasRequest obterIdeiasRequest)
        {
            var ideiasRawQuery = _ideiasRepository.ObterIdeias(obterIdeiasRequest);

            var listaIdeias = new List<IdeiasDto>();

            foreach(var ideias in ideiasRawQuery)
            {
                listaIdeias.Add(new IdeiasDto
                {
                    Codigo = ideias.Codigo,
                    Titulo = ideias.Titulo,
                    Descricao = ideias.Descricao,
                    Upvotes = ideias.Upvotes,
                    Downvotes = ideias.Downvotes,
                    NomeUsuario = ideias.NomeUsuario,
                    Comentarios = ObterComentariosPorCodigoIdeia(ideias.Codigo)
                });
            }

            return new ListarIdeiasResponse { Ideias = listaIdeias };
        }

        private List<ComentarioDto> ObterComentariosPorCodigoIdeia(Guid codigo)
        {
            return _comentarioRepository.ObterComentariosPorIdeia(codigo);
        }

        public void RegistrarIdeia(RegistrarIdeiaRequest registrarIdeiaRequest, Usuario? usuario)
        {
            if (usuario is null)
                throw new VoteioException("Usuário não identificado");

            var ideia = new Ideias
            {
                CodigoUsuario = usuario.Codigo,
                Titulo = registrarIdeiaRequest.Titulo,
                Descricao = registrarIdeiaRequest.Descricao,
                Situacao = Messaging.Enums.Situacao.Ativo
            };

            _ideiasRepository.InserirIdeia(ideia);
        }

        public void RegistrarComentario(RegistrarComentarioRequest registrarComentarioRequest, Usuario? usuario)
        {
            if (usuario is null)
                throw new VoteioException("Usuário não identificado");

            var comentario = new Comentario
            {
                CodigoIdeia = registrarComentarioRequest.CodigoIdeia,
                CodigoUsuario = usuario.Codigo,
                Conteudo = registrarComentarioRequest.Texto
            };

            _comentarioRepository.InserirComentario(comentario);
        } 

        public void AvaliarIdeia(AvaliarIdeiaRequest avaliarIdeiaRequest, Usuario? usuario)
        {
            if (usuario is null)
                throw new VoteioException("Usuário não identificado");

            ValidarUsuarioParaAvaliacao(usuario.Codigo, avaliarIdeiaRequest.CodigoIdeia);

            var vote = new Votes
            {
                CodigoIdeia = avaliarIdeiaRequest.CodigoIdeia,
                CodigoUsuario = usuario.Codigo,
                TipoVote = avaliarIdeiaRequest.TipoVote
            };

            _votesRepository.InserirAvaliacao(vote);
        }

        public void DeletarVote(DeletarVoteRequest request, Usuario? usuario)
        {
            if (usuario is null)
                throw new VoteioException("Usuário não identificado");

            var vote = _votesRepository.ObterPorIdeiaEUsuario(usuario.Codigo, request.CodigoIdeia);

            _votesRepository.Deletar(vote);
        }

        private void ValidarUsuarioParaAvaliacao(Guid codigoUsuario, Guid codigoIdeia)
        {
            var validarSeJaFoiVotado = _votesRepository.ValidarSeJaFoiVotadoPorUsuario(codigoUsuario, codigoIdeia);

            if (validarSeJaFoiVotado.Count > 0)
                throw new VoteioException("Não é possível votar mais uma vez");
        }
    }
}
