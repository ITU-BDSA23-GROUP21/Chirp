//integration test
namespace Chirp.CLI_test {
    public class TestAddCheep
    {
        #region Sample_TestCode
        [Theory]
        [InlineData("name1", "generic test", 111)] //data to be tested in genericTest()
        [InlineData("!=)", "generic test   ", 0000)]


        static void genericTest(String name, String message, long timestamp) {


            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime(); //converting input long to dateTime
            Cheep cp1 = new Cheep(name, message, timestamp); //making cheep to be tested
            List<Cheep> cheeps = new List<Cheep>{cp1}; // inserting cheep in iterable list

            String expected = $"{name}, {timestamp}, \"{message}\""; //making the expected result
            String actual = File.ReadLines("../../data/chirp_cli_db.csv");

            Assert.Equal(expected, actual)
        }
        #endregion
    }
}