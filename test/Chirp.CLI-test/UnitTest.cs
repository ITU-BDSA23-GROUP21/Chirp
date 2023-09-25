
using Chirp.Shared;

namespace Chirp.CLI_test
{
    public class UnitTest
    {

        #region Cheep_ToString
        [Theory]
        [InlineData("name1", "generic test", 111)] //data to be tested in genericTest()
        [InlineData("!=)  ", "generic test   ", 0000)]
        static void is_cheep_made_correctly_into_string(String name, String message, long time)
        {
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime(); //converting input long to dateTime
            String expected = $"{name} @ {dateTime:dd/mm/yy HH:mm:ss}: {message}"; //making the expected result
            Cheep cp1 = new Cheep(name, message, time); //making cheep to be tested
            List<Cheep> cheeps = new List<Cheep> { cp1 }; // inserting cheep in iterable list
            String actual = cp1.ToString();

            Assert.Equal(expected, actual);
        }
        #endregion

        #region Print_cheeps
        [Theory]
        [InlineData("name1", "generic test", 111)] //data to be tested in genericTest()
        [InlineData("!=)", "generic test   ", 0000)]
        static void is_result_in_terminal_equal_to_printed_cheep(String name, String message, long time)
        {
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime(); //converting input long to dateTime
            String expected = $"{name} @ {dateTime:dd/mm/yy HH:mm:ss}: {message}\n"; //making the expected result
            Cheep cp1 = new Cheep(name, message, time); //making cheep to be tested
            List<Cheep> cheeps = new List<Cheep> { cp1 }; // inserting cheep in iterable list
            String actual;
            //keeping track of what is written in the terminal
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                UserInterface.PrintCheeps(cheeps);
                actual = stringWriter.ToString();
            }
            Assert.Equal(expected, actual);
        }
        #endregion


    }
}
