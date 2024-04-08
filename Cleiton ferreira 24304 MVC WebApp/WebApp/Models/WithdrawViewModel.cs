using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class WithdrawViewModel
    {
        [Required(ErrorMessage = "Account Number is required")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; }
    }
}
