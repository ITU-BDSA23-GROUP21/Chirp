namespace SimpleDB {
    using CsvHelper;
    using CsvHelper.Configuration;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
        private string pathToCSV;
        private string testPath;
        private static CSVDatabase<T>? instance = null;

        private CSVDatabase() {
            pathToCSV = "../../data/chirp_cli_db.csv";
            testPath = "../../../../../data/chirp_cli_db.csv";
        }

        public static CSVDatabase<T> Instance {
            get {
                if (instance == null) {
                    instance = new CSVDatabase<T>();
                }
                return instance;
            }
        }

        public IEnumerable<T> Read(bool test, int? limit = null) {
            // A new CsvConfiguration is needed when not using default configuration
            // In this configuration, the header is taken in lower case, so that it is not case sensitive
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            string path = "";
            if(test){
                path = testPath;
            }else {
                path = pathToCSV;
            }
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, config)) {
                var records = csv.GetRecords<T>();
                var recordsToReturn = records.ToList();

                // If no limit is given, the program returns all records
                if (limit == null) {
                    return recordsToReturn;
                }
                else {
                    // This is used to return 'limit' amount of records
                    // This works for all enumerable objects
                    return recordsToReturn.Take((int)limit);
                }
            }
        }

        public void Store(bool test, T record) {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, // This tells the write to not duplicate the header when storing new records
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            string path = "";
            if(test){
                path = testPath;
            }else {
                path = pathToCSV;
            }
            using (var stream = File.Open(path, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config)) {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}
