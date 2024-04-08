using System;
using System.IO;

public abstract class Account
{
    // Properties for account details
    public string AccountNumber { get; protected set; }
    public decimal Balance { get; protected set; }

    // Constructor
    protected Account(string accountNumber)
    {
        AccountNumber = accountNumber;
        Balance = GetAccountBalance();
    }

    // Abstract methods for deposit and withdrawal
    public abstract void Deposit(decimal amount);
    public abstract void Withdraw(decimal amount);

    // Method to retrieve account balance from file
    protected decimal GetAccountBalance()
    {
        string filePath = $"{AccountNumber}.txt";
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
    protected void RecordTransaction(string action, decimal amount, decimal finalBalance)
    {
        string filePath = $"{AccountNumber}.txt";
        string transactionRecord = $"{DateTime.Now}\t{action}\t{amount}\t{finalBalance}";
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(transactionRecord);
        }
    }
}

public class SavingsAccount : Account
{
    // Constructor to initialize savings account
    public SavingsAccount(string accountNumber) : base(accountNumber)
    {
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
}

public class CurrentAccount : Account
{
    // Constructor to initialize current account
    public CurrentAccount(string accountNumber) : base(accountNumber)
    {
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
}
