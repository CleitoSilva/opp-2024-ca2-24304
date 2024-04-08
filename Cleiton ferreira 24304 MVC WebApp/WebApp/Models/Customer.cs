using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "PIN is required")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "PIN must be 4 characters")]
        public string Pin { get; set; }
    }
}
