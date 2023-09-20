
namespace Chirp.CLI_test {
    public class TestAddCheep {



        #region Sample_TestCode
        
        [Theory]
        [InlineData("generic test")] //data to be tested in genericTest() *the cheep message*
        [InlineData("æøå testing!")]

        static void AddCheep_addGenericCheeps_integration(String message) {
            DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
            String actual = new String("");

            Program.setDB();
            Program.AddCheep(message);

            IEnumerable<String> lines = File.ReadLines("../../../../../data/chirp_cli_db.csv");
            List<String> newLines = lines.ToList();
            actual = newLines[newLines.Count - 1]; //choosing the last record in csv file

            String expected = $"{Environment.UserName},{message},{dto.ToUnixTimeSeconds()}";

            Assert.Equal(expected, actual);
        }
        #endregion

        #region Sample_TestCode
        [Theory]
        [InlineData("name1", "generic test", 111)] //data to be tested in genericTest()
        [InlineData("!=)  ", "generic test   ", 0000)]


        static void ToString_generic_unit(String name, String message, long time) {
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime(); //converting input long to dateTime
            String expected = $"{name} @ {dateTime:dd/mm/yy HH:mm:ss}: {message}"; //making the expected result
            Cheep cp1 = new Cheep(name, message, time); //making cheep to be tested
            List<Cheep> cheeps = new List<Cheep>{cp1}; // inserting cheep in iterable list
            String actual = cp1.ToString();

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
