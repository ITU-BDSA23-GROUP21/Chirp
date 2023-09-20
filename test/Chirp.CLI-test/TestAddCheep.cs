//integration test
namespace Chirp.CLI_test {
    public class TestAddCheep {
        #region Sample_TestCode
        
        [Theory]
        [InlineData("null", "generic test", 000)] //data to be tested in genericTest() *the cheep message*
       [InlineData("null", "generic test !)¤(%)", 000)]



        static void genericTest(String name, String message, long timestamp ) {

            //IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>("../../../../../data/chirp_cli_db.csv");

            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime(); //converting input long to dateTime

            //String expected = $"{name}, {message}, \"{timestamp}\""; //making the expected result
            //string actual =  File.ReadLines("data/chirp_cli_db.csv/").Last();
            String actual = new String("");

            Program.setDB();
            Program.AddCheep(message);
            IEnumerable<String> line = File.ReadLines("../../../../../data/chirp_cli_db.csv");
            List<String> newLine = line.ToList();
            //actual = newLine.Reverse().Skip(1).FirstOrDefault();
            actual = newLine[newLine.Count - 2];



            Assert.Contains(message, actual);
        }
        #endregion
    }
}
