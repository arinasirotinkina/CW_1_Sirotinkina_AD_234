using CW_1_Sirotinkina_AD_234.Facade;
using CW_1_Sirotinkina_AD_234.Interfaces;

namespace CW_1_Sirotinkina_AD_234.DataExport;

public abstract class DataExporter
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
    public void Export(FinanceFacade facade, string filePath)
    {
        if (!CheckFileExists(filePath)) return;
        
        var visitor = CreateVisitor();

        // Обходим все доменные объекты через Visitor
        foreach (var account in facade.GetBankAccounts())
            account.Accept(visitor);
        foreach (var category in facade.GetCategories())
            category.Accept(visitor);
        foreach (var op in facade.GetOperations())
            op.Accept(visitor);

        // Форматируем данные в строку
        string formattedData = FormatData(visitor);
            
        // Записываем в файл
        File.WriteAllText(filePath, formattedData);
        Console.WriteLine($"Данные экспортированы в {filePath}");
    }

    // Фабричный метод для создания Visitor'а.
    protected abstract IVisitor CreateVisitor();

    // Абстрактный метод для форматирования данных, собранных Visitor'ом.
    protected abstract string FormatData(IVisitor visitor);
}