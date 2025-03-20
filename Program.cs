using Microsoft.Extensions.DependencyInjection;
using CW_1_Sirotinkina_AD_234.DataExport;
using CW_1_Sirotinkina_AD_234.DataImport;
using CW_1_Sirotinkina_AD_234.Domain;
using CW_1_Sirotinkina_AD_234.Facade;
using CW_1_Sirotinkina_AD_234.Infrastructure;
using CW_1_Sirotinkina_AD_234.Observer;

namespace Yahyaev_SD_IHW_1
{
    class Program
    {
        private static readonly ServiceCollection Services = new ServiceCollection();
        private static ServiceProvider _serviceProvider;
        private static FinanceFacade _facade;
        
        // Экспортеры
        private static JSONDataExporter _jsonExporter;
        private static CSVDataExporter _csvExporter;
        private static YAMLDataExporter _yamlExporter;
        
        // Импортёры
        private static JSONDataImporter _jsonImporter;
        private static CSVDataImporter _csvImporter;
        private static YAMLDataImporter _yamlImporter;

        static void ConfigureServices()
        {
            Services.AddSingleton<IBankAccountObserver, BalanceLogger>();
            Services.AddSingleton<IFinanceRepository, FinanceRepository>();
            Services.AddSingleton<FinanceFacade>();
            Services.AddTransient<JSONDataExporter>();
            Services.AddTransient<CSVDataExporter>();
            Services.AddTransient<YAMLDataExporter>();
            Services.AddTransient<JSONDataImporter>();
            Services.AddTransient<CSVDataImporter>();
            Services.AddTransient<YAMLDataImporter>();
            
            _serviceProvider = Services.BuildServiceProvider();
            _facade = _serviceProvider.GetRequiredService<FinanceFacade>();
            
            _jsonExporter = _serviceProvider.GetRequiredService<JSONDataExporter>();
            _csvExporter = _serviceProvider.GetRequiredService<CSVDataExporter>();
            _yamlExporter = _serviceProvider.GetRequiredService<YAMLDataExporter>();

            _jsonImporter = _serviceProvider.GetRequiredService<JSONDataImporter>();
            _csvImporter = _serviceProvider.GetRequiredService<CSVDataImporter>();
            _yamlImporter = _serviceProvider.GetRequiredService<YAMLDataImporter>();
        }


        static void Main(string[] args)
        {
            ConfigureServices();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Создать счет");
                Console.WriteLine("2. Создать категорию");
                Console.WriteLine("3. Создать операцию");
                Console.WriteLine("4. Показать счета");
                Console.WriteLine("5. Показать категории");
                Console.WriteLine("6. Показать операции");
                Console.WriteLine("7. Экспорт данных (JSON)");
                Console.WriteLine("8. Экспорт данных (CSV)");
                Console.WriteLine("9. Экспорт данных (YAML)");
                Console.WriteLine("10. Импорт данных (JSON)");
                Console.WriteLine("11. Импорт данных (CSV)");
                Console.WriteLine("12. Импорт данных (YAML)");
                Console.WriteLine("13. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название счета: ");
                        string accName = Console.ReadLine();
                        var newAccount = _facade.CreateBankAccount(accName);
                        Console.WriteLine($"Создан счет: {newAccount.Id} - {newAccount.Name}");
                        break;
                    case "2":
                        Console.Write("Введите название категории: ");
                        string catName = Console.ReadLine();
                        Console.Write("Введите тип категории (income/expense): ");
                        string catTypeStr = Console.ReadLine();
                        var catType = catTypeStr.ToLower() == "income" ? Category.CategoryType.Income : Category.CategoryType.Expense;
                        var newCategory = _facade.CreateCategory(catType, catName);
                        Console.WriteLine($"Создана категория: {newCategory.Id} - {newCategory.Name} ({newCategory.Type})");
                        break;
                    case "3":
                        Console.Write("Введите тип операции (income/expense): ");
                        string opTypeStr = Console.ReadLine();
                        var opType = opTypeStr.ToLower() == "income" ? Operation.OperationType.Income : Operation.OperationType.Expense;
                        
                        Console.Write("Введите сумму операции: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            Console.WriteLine("Неверное значение суммы.");
                            break;
                        }
                        
                        Console.Write("Введите описание: ");
                        string description = Console.ReadLine();
                        
                        var accounts = _facade.GetBankAccounts();
                        if (accounts.Count == 0)
                        {
                            Console.WriteLine("Нет доступных счетов. Сначала создайте счет.");
                            break;
                        }
                        Console.WriteLine("Выберите счет:");
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {accounts[i].Name} - Баланс: {accounts[i].Balance}");
                        }
                        if (!int.TryParse(Console.ReadLine(), out int accountIndex) || accountIndex < 1 || accountIndex > accounts.Count)
                        {
                            Console.WriteLine("Неверный выбор счета.");
                            break;
                        }
                        var opAccount = accounts[accountIndex - 1];

                        var categories = _facade.GetCategories().FindAll(c => c.Type.ToString().ToLower() == opTypeStr.ToLower());
                        if (categories.Count == 0)
                        {
                            Console.WriteLine("Нет подходящих категорий для данной операции. Сначала создайте категорию.");
                            break;
                        }
                        Console.WriteLine("Выберите категорию:");
                        for (int i = 0; i < categories.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {categories[i].Name}");
                        }
                        if (!int.TryParse(Console.ReadLine(), out int catIndex) || catIndex < 1 || catIndex > categories.Count)
                        {
                            Console.WriteLine("Неверный выбор категории.");
                            break;
                        }
                        var opCategory = categories[catIndex - 1];
                        
                        _facade.CreateOperation(opType, opAccount, amount, DateTime.Now, description, opCategory);
                        Console.WriteLine("Операция создана.");
                        break;
                    case "4":
                        DisplayAccounts(_facade);
                        break;
                    case "5":
                        DisplayCategories(_facade);
                        break;
                    case "6":
                        DisplayOperations(_facade);
                        break;
                    case "7":
                        Console.Write("Введите путь для экспорта (например, export_data.json): ");
                        string jsonPath = Console.ReadLine();
                        _jsonExporter.Export(_facade, jsonPath);
                        break;
                    case "8":
                        Console.Write("Введите путь для экспорта (например, export_data.csv): ");
                        string csvPath = Console.ReadLine();
                        _csvExporter.Export(_facade, csvPath);
                        break;
                    case "9":
                        Console.Write("Введите путь для экспорта (например, export_data.yaml): ");
                        string yamlPath = Console.ReadLine();
                        _yamlExporter.Export(_facade, yamlPath);
                        break;
                    case "10":
                        Console.Write("Введите путь для импорта JSON (например, import_data.json): ");
                        string importJsonPath = Console.ReadLine();
                        _jsonImporter.Import(importJsonPath, _facade);
                        Console.WriteLine("Импорт JSON завершен.");
                        break;
                    case "11":
                        Console.Write("Введите путь для импорта CSV (например, import_data.csv): ");
                        string importCsvPath = Console.ReadLine();
                        _csvImporter.Import(importCsvPath, _facade);
                        Console.WriteLine("Импорт CSV завершен.");
                        break;
                    case "12":
                        Console.Write("Введите путь для импорта YAML (например, import_data.yaml): ");
                        string importYamlPath = Console.ReadLine();
                        _yamlImporter.Import(importYamlPath, _facade);
                        Console.WriteLine("Импорт YAML завершен.");
                        break;
                    case "13":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        private static void DisplayData(FinanceFacade facade)
        {
            Console.WriteLine("\n--- Счета ---");
            DisplayAccounts(facade);
            Console.WriteLine("\n--- Категории ---");
            DisplayCategories(facade);
            Console.WriteLine("\n--- Операции ---");
            DisplayOperations(facade);
        }

        private static void DisplayAccounts(FinanceFacade facade)
        {
            foreach (var acc in facade.GetBankAccounts())
            {
                Console.WriteLine($"{acc.Id} - {acc.Name} - Баланс: {acc.Balance}");
            }
        }
        
        private static void DisplayCategories(FinanceFacade facade)
        {
            foreach (var cat in facade.GetCategories())
            {
                Console.WriteLine($"{cat.Id} - {cat.Type} - {cat.Name}");
            }
        }

        private static void DisplayOperations(FinanceFacade facade)
        {
            foreach (var op in facade.GetOperations())
            {
                Console.WriteLine($"{op.Id} - {op.Type} {op.Amount} на {op.Date:yyyy-MM-dd} (Счет: {op.BankAccount.Name}, Категория: {op.Category.Name})");
            }
        }
    }
}