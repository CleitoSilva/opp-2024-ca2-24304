// Author: Cleiton Ferreira
// Student ID: 24304

using System;
using System.Collections.Generic;
using System.IO;

// Define Transaction class
public class Transaction
{
    // Properties for transaction details
    public DateTime Date { get; set; }
    public string Action { get; set; }
    public decimal Amount { get; set; }
    public decimal FinalBalance { get; set; }

    // Constructor to initialize transaction properties
    public Transaction(DateTime date, string action, decimal amount, decimal finalBalance)
    {
        Date = date;
        Action = action;
        Amount = amount;
        FinalBalance = finalBalance;
    }
}

// Define Account class
public abstract class Account
{
    // Properties for account details
    public string AccountNumber { get; protected set; }
    public decimal Balance { get; protected set; }

    // Abstract methods for deposit and withdrawal
    public abstract void Deposit(decimal amount);
    public abstract void Withdraw(decimal amount);
}

// Define SavingsAccount class
public class SavingsAccount : Account
{
    // Constructor to initialize savings account
    public SavingsAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = GetAccountBalance();
    }

    // Method to deposit money into savings account
    public override void Deposit(decimal amount)
    {
        Balance += amount;
        RecordTransaction("Deposit", amount, Balance);
    }

    // Method to withdraw money from savings account
    public override void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            RecordTransaction("Withdrawal", amount, Balance);
        }
        else
        {
            Console.WriteLine("Insufficient balance.");
        }
    }

    // Method to retrieve account balance from file
    private decimal GetAccountBalance()
    {
        string filePath = $"{AccountNumber}.savings.txt";
        if (File.Exists(filePath))
        {
            string[] transactions = File.ReadAllLines(filePath);
            if (transactions.Length > 0)
            {
                string[] fields = transactions[transactions.Length - 1].Split('\t');
                return decimal.Parse(fields[3]);
            }
        }
        return 0;
    }

    // Method to record transaction details in file
    private void RecordTransaction(string action, decimal amount, decimal finalBalance)
    {
        string filePath = $"{AccountNumber}.savings.txt";
        string transactionRecord = $"{DateTime.Now}\t{action}\t{amount}\t{finalBalance}";
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(transactionRecord);
        }
    }
}

// Define CurrentAccount class
public class CurrentAccount : Account
{
    // Constructor to initialize current account
    public CurrentAccount(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = GetAccountBalance();
    }

    // Method to deposit money into current account
    public override void Deposit(decimal amount)
    {
        Balance += amount;
        RecordTransaction("Deposit", amount, Balance);
    }

    // Method to withdraw money from current account
    public override void Withdraw(decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            RecordTransaction("Withdrawal", amount, Balance);
        }
        else
        {
            Console.WriteLine("Insufficient balance.");
        }
    }

    // Method to retrieve account balance from file
    private decimal GetAccountBalance()
    {
        string filePath = $"{AccountNumber}.current.txt";
        if (File.Exists(filePath))
        {
            string[] transactions = File.ReadAllLines(filePath);
            if (transactions.Length > 0)
            {
                string[] fields = transactions[transactions.Length - 1].Split('\t');
                return decimal.Parse(fields[3]);
            }
        }
        return 0;
    }

    // Method to record transaction details in file
    private void RecordTransaction(string action, decimal amount, decimal finalBalance)
    {
        string filePath = $"{AccountNumber}.current.txt";
        string transactionRecord = $"{DateTime.Now}\t{action}\t{amount}\t{finalBalance}";
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(transactionRecord);
        }
    }
}

// Define Customer class
public class Customer
{
    // Properties for customer details
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AccountNumber { get; set; }
    public string Pin { get; set; }

    // Constructor to initialize customer details
    public Customer(string firstName, string lastName, string accountNumber, string pin)
    {
        FirstName = firstName;
        LastName = lastName;
        AccountNumber = accountNumber;
        Pin = pin;
    }
}

// Define BankEmployee class
public class BankEmployee
{
    // Admin PIN constant
    private const string AdminPin = "A1234";
    // List to store customer details
    public static List<Customer> customers = new List<Customer>();

    // Method to authenticate bank employee login
    public static bool Login(string pin)
    {
        return pin == AdminPin;
    }

    // Method to create a new customer
    public static void CreateCustomer(string firstName, string lastName, string email)
    {
        string accountNumber = GenerateAccountNumber(firstName, lastName);
        string pin = GeneratePin(firstName, lastName);
        customers.Add(new Customer(firstName, lastName, accountNumber, pin));
        Console.WriteLine("Customer created successfully.");

        // Write customer details to customers.txt file
        string customerDetails = $"{firstName},{lastName},{email},{accountNumber},{pin}";
        using (StreamWriter writer = File.AppendText("customers.txt"))
        {
            writer.WriteLine(customerDetails);
        }

        CreateAccountFiles(accountNumber);
    }

    // Method to delete an existing customer
    public static void DeleteCustomer(string accountNumber)
    {
        Customer customer = customers.Find(c => c.AccountNumber == accountNumber);
        if (customer != null)
        {
            if (CanDeleteCustomer(accountNumber))
            {
                customers.Remove(customer);
                DeleteAccountFiles(accountNumber);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Cannot delete customer with non-zero balance.");
            }
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }

    // Method to delete account files associated with a customer
    private static void DeleteAccountFiles(string accountNumber)
    {
        string savingsFilePath = $"{accountNumber}.savings.txt";
        string currentFilePath = $"{accountNumber}.current.txt";

        if (File.Exists(savingsFilePath))
        {
            File.Delete(savingsFilePath);
        }

        if (File.Exists(currentFilePath))
        {
            File.Delete(currentFilePath);
        }
    }

    // Method to deposit money into an account
    public static void Deposit(string accountNumber, decimal amount, string accountType)
    {
        Account account = GetAccount(accountNumber, accountType);
        if (account != null)
        {
            account.Deposit(amount);
            Console.WriteLine($"Deposit of {amount:C} to {accountType} account successful.");
        }
    }

    // Method to withdraw money from an account
    // Method to withdraw money from an account
    public static void Withdraw(string accountNumber, decimal amount, string accountType)
    {
        // Retrieve the account based on the account number and type
        Account account = GetAccount(accountNumber, accountType);
        if (account != null)
        {
            // Perform withdrawal operation if the account exists
            account.Withdraw(amount);
            Console.WriteLine($"Withdrawal of {amount:C} from {accountType} account successful.");
        }
    }

    // Method to list all customers
    public static void ListCustomers()
    {
        Console.WriteLine("List of Customers:");
        foreach (Customer customer in customers)
        {
            Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}, Account Number: {customer.AccountNumber}");
        }
    }

    // Method to retrieve a customer based on provided details
    public static Customer GetCustomer(string firstName, string lastName, string accountNumber, string pin)
    {
        return customers.Find(c => c.FirstName == firstName && c.LastName == lastName && c.AccountNumber == accountNumber && c.Pin == pin);
    }

    // Method to check if a customer's account balance is zero to enable deletion
    // Method to check if a customer's account balance is zero to enable deletion
    private static bool CanDeleteCustomer(string accountNumber)
    {
        // Initialize total balance to zero
        decimal totalBalance = 0;
        string savingsFilePath = $"{accountNumber}.savings.txt";
        string currentFilePath = $"{accountNumber}.current.txt";

        // Check if savings account file exists and update total balance
        if (File.Exists(savingsFilePath))
        {
            totalBalance += GetAccountBalance(savingsFilePath);
        }

        // Check if current account file exists and update total balance
        if (File.Exists(currentFilePath))
        {
            totalBalance += GetAccountBalance(currentFilePath);
        }

        // Return true if the total balance is zero, indicating it's safe to delete the customer
        return totalBalance == 0;
    }

    // Method to calculate the total balance of an account from transaction records
    private static decimal GetAccountBalance(string filePath)
    {
        // Initialize balance to zero
        decimal balance = 0;
        string[] transactions = File.ReadAllLines(filePath);

        // If there are transactions recorded
        if (transactions.Length > 0)
        {
            // Extract the balance from the last recorded transaction
            string[] fields = transactions[transactions.Length - 1].Split('\t');
            balance = decimal.Parse(fields[3]);
        }

        return balance;
    }

    // Method to retrieve an account based on account number and type
    private static Account GetAccount(string accountNumber, string accountType)
    {
        // Construct file path based on account number and type
        string filePath = $"{accountNumber}.{accountType}.txt";

        // If the file exists
        if (File.Exists(filePath))
        {
            // Create and return the corresponding account object based on account type
            if (accountType == "savings")
            {
                return new SavingsAccount(accountNumber);
            }
            else if (accountType == "current")
            {
                return new CurrentAccount(accountNumber);
            }
        }

        // If the file doesn't exist, inform that the account was not found
        Console.WriteLine($"{accountType} Account not found.");
        return null;
    }

    // Method to create account files if they don't exist
    private static void CreateAccountFiles(string accountNumber)
    {
        // Construct file paths for savings and current accounts
        string savingsFilePath = $"{accountNumber}.savings.txt";
        string currentFilePath = $"{accountNumber}.current.txt";

        // If savings account file doesn't exist, create it
        if (!File.Exists(savingsFilePath))
        {
            File.Create(savingsFilePath).Close();
        }

        // If current account file doesn't exist, create it
        if (!File.Exists(currentFilePath))
        {
            File.Create(currentFilePath).Close();
        }
    }

    // Method to generate a unique account number based on customer name
    private static string GenerateAccountNumber(string firstName, string lastName)
    {
        // Combine initials, name length, and initial positions to form a unique account number
        string initials = $"{firstName[0]}{lastName[0]}".ToLower();
        int nameLength = firstName.Length + lastName.Length;
        int firstInitialPosition = firstName[0] - 'a' + 1;
        int secondInitialPosition = lastName[0] - 'a' + 1;
        return $"{initials}-{nameLength}-{firstInitialPosition}-{secondInitialPosition}";
    }

    // Method to generate a PIN based on customer name
    private static string GeneratePin(string firstName, string lastName)
    {
        // Calculate positions of first initials to form a PIN
        int firstInitialPosition = firstName[0] - 'a' + 1;
        int secondInitialPosition = lastName[0] - 'a' + 1;
        return $"{firstInitialPosition}{secondInitialPosition}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Create customers file if it doesn't exist
        if (!File.Exists("customers.txt"))
        {
            File.Create("customers.txt").Close();
        }

        // Main menu loop
        string input;
        do
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Bank Employee Login");
            Console.WriteLine("2. Customer Login");
            Console.WriteLine("3. Exit");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    BankEmployeeLogin();
                    break;
                case "2":
                    CustomerLogin();
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (input != "3");
    }

    // Method for bank employee login
    public static void BankEmployeeLogin()
    {
        Console.WriteLine("Enter Bank Employee PIN:");
        string pin = Console.ReadLine();
        if (BankEmployee.Login(pin))
        {
            Console.WriteLine("Login successful.");
            BankEmployeeMenu();
        }
        else
        {
            Console.WriteLine("Invalid PIN.");
        }
    }

    // Method for bank employee menu
    public static void BankEmployeeMenu()
    {
        // Bank employee menu loop
        string input;
        do
        {
            Console.WriteLine("Bank Employee Menu:");
            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Delete Customer");
            Console.WriteLine("3. Deposit Money");
            Console.WriteLine("4. Withdraw Money");
            Console.WriteLine("5. List Customers");
            Console.WriteLine("6. Back to Main Menu");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Enter Customer Details:");
                    Console.WriteLine("First Name:");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("Last Name:");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string email = Console.ReadLine();
                    BankEmployee.CreateCustomer(firstName, lastName, email);
                    break;
                case "2":
                    Console.WriteLine("Enter Customer Account Number to Delete:");
                    string accountNumToDelete = Console.ReadLine();
                    BankEmployee.DeleteCustomer(accountNumToDelete);
                    break;
                case "3":
                    DepositOrWithdraw("Deposit");
                    break;
                case "4":
                    DepositOrWithdraw("Withdraw");
                    break;
                case "5":
                    BankEmployee.ListCustomers();
                    break;
                case "6":
                    Console.WriteLine("Returning to Main Menu...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (input != "6");
    }

    // Method for depositing or withdrawing money
    public static void DepositOrWithdraw(string action)
    {
        Console.WriteLine("Enter Customer Account Number:");
        string accountNumber = Console.ReadLine();
        Console.WriteLine("Enter Amount:");
        decimal amount;
        if (decimal.TryParse(Console.ReadLine(), out amount))
        {
            Console.WriteLine("Choose Account Type (savings or current):");
            string accountType = Console.ReadLine().ToLower();
            if (action == "Deposit")
            {
                BankEmployee.Deposit(accountNumber, amount, accountType);
            }
            else if (action == "Withdraw")
            {
                BankEmployee.Withdraw(accountNumber, amount, accountType);
            }
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }

    // Method for customer login
    public static void CustomerLogin()
    {
        // Prompt user for login credentials
        Console.WriteLine("Enter First Name:");
        string firstName = Console.ReadLine();
        Console.WriteLine("Enter Last Name:");
        string lastName = Console.ReadLine();
        Console.WriteLine("Enter Account Number:");
        string accountNumber = Console.ReadLine();
        Console.WriteLine("Enter PIN:");
        string pin = Console.ReadLine();

        // Retrieve customer based on provided details
        Customer customer = BankEmployee.GetCustomer(firstName, lastName, accountNumber, pin);
        if (customer != null)
        {
            Console.WriteLine("Login successful.");
            CustomerMenu(customer);
        }
        else
        {
            Console.WriteLine("Invalid credentials.");
        }
    }

    // Method for customer menu
    public static void CustomerMenu(Customer customer)
    {
        // Customer menu loop
        string input;
        do
        {
            Console.WriteLine("Customer Menu:");
            Console.WriteLine("1. View Transaction History");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Back to Main Menu");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewTransactionHistory(customer);
                    break;
                case "2":
                    DepositOrWithdrawCustomer(customer, "Deposit");
                    break;
                case "3":
                    DepositOrWithdrawCustomer(customer, "Withdraw");
                    break;
                case "4":
                    Console.WriteLine("Returning to Main Menu...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        } while (input != "4");
    }

    // Method to view transaction history
    public static void ViewTransactionHistory(Customer customer)
    {
        Console.WriteLine("Choose Account Type (savings or current):");
        string accountType = Console.ReadLine().ToLower();
        string filePath = $"{customer.AccountNumber}.{accountType}.txt";
        if (File.Exists(filePath))
        {
            Console.WriteLine($"Transaction History for {accountType} Account:");
            DisplayTransactions(filePath);
        }
        else
        {
            Console.WriteLine($"{accountType} Account not found.");
        }
    }

    // Method to deposit or withdraw money for a customer
    public static void DepositOrWithdrawCustomer(Customer customer, string action)
    {
        Console.WriteLine("Choose Account Type (savings or current):");
        string accountType = Console.ReadLine().ToLower();
        Console.WriteLine($"Enter Amount to {action}:");
        decimal amount;
        if (decimal.TryParse(Console.ReadLine(), out amount))
        {
            if (action == "Deposit")
            {
                BankEmployee.Deposit(customer.AccountNumber, amount, accountType);
            }
            else if (action == "Withdraw")
            {
                BankEmployee.Withdraw(customer.AccountNumber, amount, accountType);
            }
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }

    // Method to display transactions from a file
    public static void DisplayTransactions(string filePath)
    {
        string[] transactions = File.ReadAllLines(filePath);
        foreach (string transaction in transactions)
        {
            Console.WriteLine(transaction);
        }
    }
}



