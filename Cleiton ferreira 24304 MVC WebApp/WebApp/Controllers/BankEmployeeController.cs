using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class BankEmployeeController : Controller
{
    private readonly ILogger<BankEmployeeController> _logger;

    public BankEmployeeController(ILogger<BankEmployeeController> logger)
    {
        _logger = logger;
    }

    public IActionResult CreateCustomer()
    {
        return View();
    }

    public IActionResult DeleteCustomer()
    {
        return View();
    }

    public IActionResult Deposit()
    {
        return View();
    }
        public IActionResult Withdraw()
    {
        return View();
    }
        public IActionResult ListCustomers()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
