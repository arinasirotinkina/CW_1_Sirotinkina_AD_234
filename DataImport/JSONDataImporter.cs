using System.Text.Json;

namespace CW_1_Sirotinkina_AD_234.DataImport;

public class JSONDataImporter : DataImporter
{
    protected override List<Dictionary<string, string>> ParseData(string content)
    {
        var records = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);
        return records ?? new List<Dictionary<string, string>>();
    }
}