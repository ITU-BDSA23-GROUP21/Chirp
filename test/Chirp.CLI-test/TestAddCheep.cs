//integration test
namespace Chirp.CLI_test {
    public class TestAddCheep {
        #region Sample_TestCode
        [Theory]
        [InlineData("generic test")] //data to be tested in genericTest() *the cheep message*
        [InlineData("generic test   ")]

        DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
        dto.ToUnixTimeSeconds()

        static void genericTest(Environment.UserName, String message, dto.ToUnixTimeSeconds()) {

            //IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>("../../../../../data/chirp_cli_db.csv");

            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime(); //converting input long to dateTime

            String expected = $"{name}, {message}, \"{timestamp}\""; //making the expected result
            //string actual =  File.ReadLines("data/chirp_cli_db.csv/").Last();
            String actual = new String("");

            /*Program.AddCheep(message); */
            Program.AddCheep(message);
            String[] line = (string[])File.ReadLines("../../../../../data/chirp_cli_db.csv")
            actual = line[line.length - 2]



            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
