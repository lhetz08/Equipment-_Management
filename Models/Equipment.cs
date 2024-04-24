using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment__Management.Models
{
    public class Equipment
    {        

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Equipment_Id { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public required string Serial_Number { get; set; }       
        public string? Description { get; set; }
        public string Condition { get; set; }
        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public User User { get; set; }
    }

    internal enum Enum_EquipmentCondition
    {
       None = 0,
       Bad = 1,
       Good = 2       
    }
}
