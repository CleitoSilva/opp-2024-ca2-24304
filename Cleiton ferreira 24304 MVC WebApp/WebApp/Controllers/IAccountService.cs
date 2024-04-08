using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IAccountService
    {
        // Customer related methods
        Customer GetCustomer(string firstName, string lastName, string accountNumber, string pin);
        Customer GetCustomerById(int customerId);

        // Transaction related methods
        IEnumerable<Transaction> GetTransactionHistory(string accountNumber, string accountType);
        void Deposit(string accountNumber, decimal amount, string accountType);
        void Withdraw(string accountNumber, decimal amount, string accountType);
    }
}
