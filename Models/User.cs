using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment__Management.Models
{
    public class User
    {     

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int User_Id { get; set; }
        public string User_Type { get; set; }
        public string First_Name { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string Last_Name { get; set; }
        public string Email_Address { get; set; } = string.Empty;
        public string User_Name { get; set; }
        public string Password { get; set; }

        public string FullName { get => $"{ Last_Name }, { First_Name }"; }

        public ICollection<Site> Sites { get; set; }
    }

    internal enum Enum_UserType
    {
        None = 0,
        Admin = 1,
         SuperAdmin = 2
    }    

    public class UserTypeModel
    {
        public string Description { get; set; }
    }
}
