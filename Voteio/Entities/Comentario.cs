using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Voteio.Entities
{
    public class Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(36)]
        public Guid Codigo { get; set; } = Guid.NewGuid(); 

        [Required]
        [StringLength(36)]
        public Guid CodigoIdeia { get; set; }

        [Required]
        [StringLength(36)]
        public Guid CodigoUsuario { get; set; }

        [Required]
        public string Conteudo { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DtInclusao { get; set; } = DateTime.Now;
    }
}
