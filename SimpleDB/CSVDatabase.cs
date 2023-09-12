using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>{
    private string pathToCSV;

    public CSVDatabase(string pathToCSV)
    {
        this.pathToCSV = pathToCSV;
    }

    public IEnumerable<T> Read(int limit){
        
    }

    public void Store(T record){
        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        using(var stream = File.Open(pathToCSV, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecord(record);
        }
    }
}
