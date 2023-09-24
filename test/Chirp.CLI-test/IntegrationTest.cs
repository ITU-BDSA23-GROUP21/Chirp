namespace Chirp.CLI_test
{
    public class IntegrationTest
    {

        #region AddCheep

        [Theory]
        [InlineData("generic test")] //data to be tested in genericTest() *the cheep message*
        [InlineData("æøå testing!")]
        static async void is_cheep_stored_correct_using_æøå(String message)
        {
            DateTimeOffset dto = DateTimeOffset.Now.ToLocalTime();
            String actual = new String("");

            Program.setDB();
            Program.setClient();
            await Program.AddCheep(message);
            //this fails since the line below tries to read the file before the line above has added the new test cheeps
            IEnumerable<String> lines = File.ReadLines("chirp_cli_db.csv");
            List<String> newLines = lines.ToList();
            actual = newLines[newLines.Count - 1]; //choosing the last record in csv file

            //String expected = $"{Environment.UserName},{message},{dto.ToUnixTimeSeconds()}";

            Assert.Contains(message, actual);

        }
        #endregion


    }
}
