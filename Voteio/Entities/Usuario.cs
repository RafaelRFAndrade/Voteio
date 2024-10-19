﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Voteio.Messaging.Enums;

namespace Voteio.Entities
{
    public class Usuario 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } 

        [StringLength(155)]
        public string Nome { get; set; } 

        [Required]
        [StringLength(155)]
        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [StringLength(255)]
        public string Senha { get; set; } 

        [Column(TypeName = "timestamp")]
        public DateTime DtInclusao { get; set; } = DateTime.Now; 

        [Column(TypeName = "timestamp")]
        public DateTime DtAtualizacao { get; set; } = DateTime.Now; 

        public Situacao Situacao { get; set; }

        [Required]
        [StringLength(36)]
        public Guid Codigo { get; set; } = Guid.NewGuid(); 
    }
}
