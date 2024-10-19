using System.ComponentModel.DataAnnotations;

namespace Voteio.Messaging.Requests
{
    public class CadastrarUsuarioRequest
    {
        public string Nome { get; set; }

        [Required]
        [StringLength(155)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Senha { get; set; }
    }
}
