//integration test
namespace Chirp.CLI_test {
    public class TestCheepToString
    {
        #region Sample_TestCode
        [Theory]
        [InlineData("name1", "generic test", 111)] //data to be tested in genericTest()
        [InlineData("!=)  ", "generic test   ", 0000)]


        static void genericTest(String name, String message, long time) {
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