using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teslow_srv.Domain.Entities
{
    [Table("tables")]
    public class GameTable
    {
        [Key]
        [Column("game_table_id")]
        [StringLength(50)]
        public string GameTableId { get; set; } = Guid.NewGuid().ToString("N");

        public ICollection<GameTableAssignment> GameAssignments { get; set; } = new List<GameTableAssignment>();
    }
}
