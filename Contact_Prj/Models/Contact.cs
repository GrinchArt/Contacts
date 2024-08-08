using System.ComponentModel.DataAnnotations;

namespace Contact_Prj.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Range(0, 1000000)] //put a random number, but if it`s necessary you can use double.MaxValue or etc. 
        public decimal Salary { get; set; }
    }
}
