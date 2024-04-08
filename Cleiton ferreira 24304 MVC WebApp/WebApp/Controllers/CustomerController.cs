using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;
using System.Linq;

namespace WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IAccountService _accountService;

        public CustomerController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Customer login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string firstName, string lastName, string accountNumber, string pin)
        {
            var customer = _accountService.GetCustomer(firstName, lastName, accountNumber, pin);

            if (customer != null)
            {
                TempData["SuccessMessage"] = "Login successful.";
                return RedirectToAction(nameof(CustomerMenu), new { customerId = customer.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid credentials.";
                return View();
            }
        }

        // Customer menu
        public IActionResult CustomerMenu(int customerId)
        {
            var customer = _accountService.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // View transaction history
        public IActionResult ViewTransactionHistory(int customerId, string accountType)
        {
            var customer = _accountService.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var transactions = _accountService.GetTransactionHistory(customer.AccountNumber, accountType);
            if (transactions != null)
            {
                return View(transactions);
            }
            else
            {
                TempData["ErrorMessage"] = $"{accountType} Account not found.";
                return RedirectToAction(nameof(CustomerMenu), new { customerId });
            }
        }

        // Deposit money
        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(string accountNumber, decimal amount, string accountType)
        {
            if (ModelState.IsValid)
            {
                _accountService.Deposit(accountNumber, amount, accountType);
                TempData["SuccessMessage"] = $"Deposit of {amount:C} to {accountType} account successful.";
                return RedirectToAction(nameof(CustomerMenu));
            }
            return View();
        }

        // Withdraw money
        [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdraw(string accountNumber, decimal amount, string accountType)
        {
            if (ModelState.IsValid)
            {
                _accountService.Withdraw(accountNumber, amount, accountType);
                TempData["SuccessMessage"] = $"Withdrawal of {amount:C} from {accountType} account successful.";
                return RedirectToAction(nameof(CustomerMenu));
            }
            return View();
        }


    }
}
