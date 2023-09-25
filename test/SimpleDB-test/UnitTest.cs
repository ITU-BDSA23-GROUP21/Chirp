
using Chirp.Shared;
using SimpleDB;

namespace Chirp.CLI_test
{
    public class TestAddCheep
    {

        #region Sample_TestCode

        [Theory]
        [InlineData("generic name", "generic test", 000)] //data to be tested in genericTest() *the cheep message*
        [InlineData("æøå", "æøå testing ¤%&", 111111)]

        // Is this a unit test? It tests multiple methods
        static async void Store_generic_integration(string name, string message, long timestamp)
        {
            var database = CSVDatabase<Cheep>.Instance;
            Cheep fakeCheep = new(name, message, timestamp);
            database.Store(fakeCheep);

            IEnumerable<string> line = File.ReadLines("chirp_cli_db.csv");
            List<string> newLines = line.ToList();
            var actual = newLines[newLines.Count - 1];
            string expected = $"{name},{message},{timestamp}";

            Assert.Equal(expected, actual);
        }
        #endregion


        #region 
        //test if read method reads the expected amount of lines from file
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(0)]

        public void Read_ReadGivenAmoutOfLinesTest_Unit(int linesToRead)
        {
            var database = CSVDatabase<Cheep>.Instance;
            var cheepsRead = database.Read(linesToRead);

            Assert.Equal(linesToRead, cheepsRead.Count());
        }
        #endregion


        #region 
        //test if the read method returns what is expected
        [Theory]
        [InlineData(1, new string[] { "Hello, BDSA students!" })]
        [InlineData(2, new string[] { "Hello, BDSA students!", "Welcome to the course!" })]
        public void ReadTest(int linesToRead, string[] expected)
        {
            // Test modified to only check message, as the timestamp test failed on github server, due to it being in a different timezone
            var database = CSVDatabase<Cheep>.Instance;
            var cheepsRead = database.Read(linesToRead);
            var i = 0;
            foreach (Cheep item in cheepsRead)
            { // converting read data to string array, so it can be compared with the expected array.
                Assert.EndsWith(expected[i], item.ToString());
                i++;
            }
        }
        #endregion


    }
}
