using Caliburn.Micro;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment__Management.Models
{
    public class Site
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Site_Id { get; set; }


        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }

        public string Description { get; set; }
        public bool Active { get; set; } = false;

        //public Site()
        //{
        //    _equipments = new List<Equipment>();
        //}

        //public BindableCollection<Equipment> _equipment = new BindableCollection<Equipment>();
        //public BindableCollection<Equipment> Equipment { 
        //    get { return _equipment; }
        //    set { _equipment = value; }
        //}

    }
}
