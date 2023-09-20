namespace SimpleDB {
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.Extensions.FileProviders;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
        private static CSVDatabase<T>? instance = null;


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
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            // There was a problem using the database, the path to the CSV file, was not the same when testing and running the program
            // Therefore we added the CSV file as an embedded file in the database project, following these links:
            // https://josef.codes/using-embedded-files-in-dotnet-core/
            // https://stackoverflow.com/questions/38762368/embedded-resource-in-net-core-libraries/57811919#57811919
            using (var reader = embeddedProvider.GetFileInfo("chirp_cli_db.csv").CreateReadStream())
            using (var sr = new StreamReader(reader))
            using (var csv = new CsvReader(sr, config)) {
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

        public void Store(T record) {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = false, // This tells the write to not duplicate the header when storing new records
                PrepareHeaderForMatch = args => args.Header.ToLower()
            };
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using (var reader = embeddedProvider.GetFileInfo("chirp_cli_db.csv").CreateReadStream())
            using (var sr = new StreamWriter(reader))
            using (var csv = new CsvWriter(sr, config)) {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}
