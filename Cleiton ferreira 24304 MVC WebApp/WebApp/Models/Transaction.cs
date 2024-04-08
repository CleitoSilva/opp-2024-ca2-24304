using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Action is required")]
        public string Action { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Final Balance is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Final Balance must be a positive number")]
        public decimal FinalBalance { get; set; }
        
        [Required(ErrorMessage = "Account Number is required")]
        public string AccountNumber { get; set; }
    }
}
