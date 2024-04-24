using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment__Management.Models
{
    public class Registered_Equipment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int Equipment_Id { get; set; }
        public int Site_Id { get; set; }

        [ForeignKey("Equipment_Id")]
        public Equipment Equipment { get; set; }

        [ForeignKey("Site_Id")]
        public Site Site { get; set; }
    }
}
