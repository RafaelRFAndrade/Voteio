namespace Voteio.Messaging.Responses
{
    public class ComentarioDto
    {
        public Guid CodigoUsuario {  get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Comentario {  get; set; }
    }
}
