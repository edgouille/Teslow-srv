using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("GameTables")]
    public class GameTable
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<GameTableAssignment> Assignments { get; set; } = new List<GameTableAssignment>();
    }
}
