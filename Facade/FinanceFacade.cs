using CW_1_Sirotinkina_AD_234.Domain;
using CW_1_Sirotinkina_AD_234.Factories;
using CW_1_Sirotinkina_AD_234.Infrastructure;
using CW_1_Sirotinkina_AD_234.Observer;

namespace CW_1_Sirotinkina_AD_234.Facade;

public class FinanceFacade(IFinanceRepository repository, IEnumerable<IBankAccountObserver> observers)
{
    public BankAccount CreateBankAccount(string name)
    {
        var account = DomainFactory.CreateBankAccount(name);
        foreach (var observer in observers)
        {
            account.OnBalanceChanged += observer.HandleBalanceChanged;
        }
        repository.AddBankAccount(account);
        return account;
    }
    
    public List<BankAccount> GetBankAccounts() => repository.GetBankAccounts();

    public void UpdateBankAccount(BankAccount account)
    {
        repository.UpdateBankAccount(account);
    }

    public void RemoveBankAccount(Guid accountId)
    {
        repository.RemoveBankAccount(accountId);
    }

    public Category CreateCategory(Category.CategoryType type, string name)
    {
        var category = DomainFactory.CreateCategory(type, name);
        repository.AddCategory(category);
        return category;
    }
    
    public List<Category> GetCategories() => repository.GetCategories();
    
    public void UpdateCategory(Category category)
    {
        repository.UpdateCategory(category);
    }

    public void RemoveCategory(Guid categoryId)
    {
        repository.RemoveCategory(categoryId);
    }

    public Operation CreateOperation(Operation.OperationType type, BankAccount account, decimal amount, DateTime date,
        string description, Category category)
    {
        var operation = DomainFactory.CreateOperation(type, account, amount, date, description, category);
        repository.AddOperation(operation);
        return operation;
    }
    
    public List<Operation> GetOperations() => repository.GetOperations();

    public void UpdateOperation(Operation operation)
    {
        repository.UpdateOperation(operation);
    }

    public void RemoveOperation(Guid operationId)
    {
        repository.RemoveOperation(operationId);
    }
    
    public decimal CalculateNetBalance(DateTime start, DateTime end)
    {
        var operations = repository.GetOperations()
            .Where(o => o.Date >= start && o.Date <= end);
        decimal income = operations
            .Where(o => o.Type == Operation.OperationType.Income)
            .Sum(o => o.Amount);
        decimal expense = operations
            .Where(o => o.Type == Operation.OperationType.Expense)
            .Sum(o => o.Amount);
        return income - expense;
    }
    
    public Dictionary<string, decimal> GroupOperationsByCategory(DateTime start, DateTime end)
    {
        var operations = repository.GetOperations()
            .Where(o => o.Date >= start && o.Date <= end);
        var groups = operations
            .GroupBy(o => o.Category.Name)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Type == Operation.OperationType.Income ? o.Amount : -o.Amount));
        return groups;
    }
    
    public void RecalculateBalance(BankAccount account)
    {
        decimal newBalance = repository.GetOperations()
            .Where(o => o.BankAccount.Id == account.Id)
            .Sum(o => o.Type == Operation.OperationType.Income ? o.Amount : -o.Amount);
        account.Balance = newBalance;
    }
}