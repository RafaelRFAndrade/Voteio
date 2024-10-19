﻿namespace Voteio.Messaging.Responses
{
    public class IdeiasDto
    {
        public Guid Codigo { get; set; }
        public string Titulo { get; set; }
        public string Descricao {  get; set; }
        public int Nota { get; set; }
        public List<ComentarioDto?> Comentarios {  get; set; }
    }
}