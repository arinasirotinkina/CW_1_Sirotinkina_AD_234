using CW_1_Sirotinkina_AD_234.Domain;
using CW_1_Sirotinkina_AD_234.Facade;

namespace CW_1_Sirotinkina_AD_234.DataImport;

public abstract class DataImporter
{
    private bool CheckFileExists(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Неверный путь к файлу: путь пустой.");
            return false;
        }
    
        var directory = System.IO.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Console.WriteLine($"Неверный путь к файлу: директория '{directory}' не существует.");
            return false;
        }

        return true;
    }
    
    public void Import(string filePath, FinanceFacade facade)
    {
        if (!CheckFileExists(filePath)) return;
        string content = File.ReadAllText(filePath);
        var records = ParseData(content);
        ProcessData(records, facade);
    }
    
    protected abstract List<Dictionary<string, string>> ParseData(string content);
    
    protected virtual void ProcessData(List<Dictionary<string, string>> records, FinanceFacade facade)
    {
        foreach (var record in records)
        {
            try
            {
                string typeStr = record["Type"];
                Operation.OperationType opType = typeStr.ToLower() == "income" ? Operation.OperationType.Income : Operation.OperationType.Expense;
                
                string accountName = record["AccountName"];
                var account = facade.GetBankAccounts().Find(a => a.Name == accountName);
                if (account == null)
                {
                    account = facade.CreateBankAccount(accountName);
                }
                
                decimal amount = decimal.Parse(record["Amount"]);
                DateTime date = DateTime.Parse(record["Date"]);
                string description = record.ContainsKey("Description") ? record["Description"] : "";
                
                string categoryName = record["CategoryName"];
                string categoryTypeStr = record["CategoryType"];
                Category.CategoryType catType = categoryTypeStr.ToLower() == "income" ? Category.CategoryType.Income : Category.CategoryType.Expense;
                
                var category = facade.GetCategories().Find(c => c.Name == categoryName && c.Type == catType);
                if (category == null)
                {
                    category = facade.CreateCategory(catType, categoryName);
                }
                
                facade.CreateOperation(opType, account, amount, date, description, category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки записи: {ex.Message}");
            }
        }
    }
}