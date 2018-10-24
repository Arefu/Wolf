using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blue_Eyes_White_Dragon.DataAccess.Models
{
    [Table("texts")]
    public class Texts
    {
        [Key]
        [Column("id", TypeName = "integer")]
        public int Id { get; set; }
        [Column("name", TypeName = "TEXT")]
        public string Name { get; set; }
    }
}