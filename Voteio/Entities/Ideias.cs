using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Voteio.Messaging.Enums;

namespace Voteio.Entities
{
    public class Ideias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(36)]
        public Guid Codigo { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(36)]
        public Guid CodigoUsuario { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime DtInclusao { get; set; } = DateTime.Now;

        [Column(TypeName = "timestamp")]
        public DateTime DtSituacao { get; set; } = DateTime.Now;

        public Situacao Situacao { get; set; }
    }
}
