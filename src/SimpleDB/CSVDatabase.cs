namespace SimpleDB {
    using CsvHelper;
    using CsvHelper.Configuration;
    using System.Globalization;

    public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
        private static CSVDatabase<T>? instance = null;

        private static string? path;


        private CSVDatabase() {
            // Path to data file differs between development and the compiled program
            // Kind of hacky solution to get the correct path in both cases
            // Devs need to use the launch profile to get correct environment, i.e. use 'dotnet run --launch-profile Development'
            var folder = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "../../data" : Directory.GetCurrentDirectory();
            path = $"{folder}/chirp_cli_db.csv";
        }

        public static CSVDatabase<T> Instance {
            get {
                if (instance == null) {
                    instance = new CSVDatabase<T>();
                }
                return instance;
            }
        }

        public IEnumerable<T> Read(int? limit = null) {
            // A new CsvConfiguration is needed when not using default configuration
            // In this configuration, the header is taken in lower case, so that it is not case sensitive
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            using (var sr = new StreamReader(path))
            using (var csv = new CsvReader(sr, config)) {
                var records = csv.GetRecords<T>();
                var recordsToReturn = records.ToList();
                // If no limit is given, the program returns all records
                if (limit == null || limit > records.Count()) {
                    return recordsToReturn;
                }
                else {
                    // This is used to return 'limit' amount of records
                    // This works for all enumerable objects
                    return recordsToReturn.Take((int)limit);
                }
            }
        }

        public void Store(T record) {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, // This tells the write to not duplicate the header when storing new records
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            using (var stream = File.Open(path, FileMode.Append))
            using (var sr = new StreamWriter(stream))
            using (var csv = new CsvWriter(sr, config)) {
                csv.WriteRecord(record);
                csv.NextRecord();
            }

        }
    }
}
