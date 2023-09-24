using Cheep;

namespace Chirp.CLI_test
{
    public class TestAddCheep
    {

        #region Sample_TestCode

        [Theory]
        [InlineData("generic name", "generic test", 000)] //data to be tested in genericTest() *the cheep message*
        [InlineData("æøå", "æøå testing ¤%&", 111111)]

        static void Store_generic_integration(String name, String message, long timestamp)
        {
            var database = Program.setDB();
            String actual = new String("");

            Cheep fakeCheep = new Cheep(name, message, timestamp);
            database.Store(fakeCheep);

            IEnumerable<String> line = File.ReadLines("chirp_cli_db.csv");
            List<String> newLines = line.ToList();
            actual = newLines[newLines.Count - 1];
            String expected = $"{name},{message},{timestamp}";

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
            var database = Program.setDB();
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
            var database = Program.setDB();
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
