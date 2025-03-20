using CW_1_Sirotinkina_AD_234.Domain;

namespace CW_1_Sirotinkina_AD_234.Infrastructure;

public class FinanceRepository : IFinanceRepository
{
    private readonly List<BankAccount> _bankAccounts = new List<BankAccount>();
    private readonly List<Category> _categories = new List<Category>();
    private readonly List<Operation> _operations = new List<Operation>();
    
    public void AddBankAccount(BankAccount bankAccount)
    {
        _bankAccounts.Add(bankAccount);
    }

    public List<BankAccount> GetBankAccounts() => _bankAccounts;

    public void UpdateBankAccount(BankAccount bankAccount) {}

    public void RemoveBankAccount(Guid bankId)
    {
        _bankAccounts.RemoveAll(a => a.Id == bankId);
        _operations.RemoveAll(o => o.BankAccount.Id == bankId);
    }

    public void AddCategory(Category category)
    {
        _categories.Add(category);
    }
    
    public List<Category> GetCategories() => _categories;

    public void UpdateCategory(Category category)
    {
    }

    public void RemoveCategory(Guid categoryId)
    {
        _bankAccounts.RemoveAll(c => c.Id == categoryId);
        _operations.RemoveAll(o => o.Category.Id == categoryId); 
    }

    public void AddOperation(Operation operation)
    {
        _operations.Add(operation);
        if (operation.Type == Operation.OperationType.Income)
            operation.BankAccount.Balance += operation.Amount;
        else if (operation.Type == Operation.OperationType.Expense)
            operation.BankAccount.Balance -= operation.Amount;
    }
    
    public List<Operation> GetOperations() => _operations;

    public void UpdateOperation(Operation operation)
    {
        var op = _operations.FirstOrDefault(o => o.Id == operation.Id);
        if (op != null)
        {
            op.Amount = operation.Amount;
            op.Date = operation.Date;
            op.Description = operation.Description;
            op.Category = operation.Category;
        }
       
        var account = op.BankAccount;
        var newBalance = _operations
            .Where(o => o.BankAccount.Id == account.Id)
            .Sum(o => o.Type == Operation.OperationType.Income ? o.Amount : -o.Amount);
        account.Balance = newBalance;
    }

    public void RemoveOperation(Guid operationId)
    {
        var op = _operations.FirstOrDefault(o => o.Id == operationId);
        if (op != null)
        {
            if (op.Type == Operation.OperationType.Income)
                op.BankAccount.Balance -= op.Amount;
            else if (op.Type == Operation.OperationType.Expense)
                op.BankAccount.Balance += op.Amount;
            
            _operations.Remove(op);
        }
    }
}