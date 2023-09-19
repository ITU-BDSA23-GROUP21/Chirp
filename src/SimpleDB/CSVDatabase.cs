namespace SimpleDB {
    using CsvHelper;
    using CsvHelper.Configuration;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
        private string pathToCSV;
        private static CSVDatabase<T> instance = null;

        private CSVDatabase() {
            pathToCSV = "../../data/chirp_cli_db.csv";
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
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config)) {
                var records = csv.GetRecords<T>();
                var recordsToReturn = records.ToList();

                if (limit == null) {
                    return recordsToReturn;
                }
                else {
                    return recordsToReturn.Skip(0).Take((int)limit);
                }
            }
        }

        public void Store(T record) {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false,
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            using (var stream = File.Open(pathToCSV, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}