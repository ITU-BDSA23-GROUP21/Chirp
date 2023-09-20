//integration test
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace Chirp.CLI_test {
    public class TestAddCheep {

        #region Sample_TestCode
    
        [Theory]
        [InlineData("generic name", "generic test", 000)] //data to be tested in genericTest() *the cheep message*
        [InlineData("æøå", "æøå testing ¤%&", 111111)]

        static void Store_generic_integration(String name, String message, long timestamp) {
            var database = Program.setDB();
            String actual = new String("");

            Cheep cp = new Cheep(name, message, timestamp);
            database.Store(cp);

            IEnumerable<String> line = File.ReadLines("../../../../../data/chirp_cli_db.csv");
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

        public void Read_ReadGivenAmoutOfLinesTest_Unit(int linesToRead) {
            var database = Program.setDB();
            var cheepsRead = database.Read(linesToRead);

            Assert.Equal(linesToRead, cheepsRead.Count());
        }
        #endregion

        #region 
        //test if the read method returns what is expected
        [Theory]
        [InlineData(1, new string[] { "ropf @ 01/09/23 14:09:20: Hello, BDSA students!" })]
        [InlineData(2, new string[] { "ropf @ 01/09/23 14:09:20: Hello, BDSA students!", "rnie @ 02/19/23 14:19:38: Welcome to the course!" })]

        public void ReadTest(int linestoread, string[] expected) {
            var database = Program.setDB();
            var cheepsRead = database.Read(linestoread);
            string[] cheeps = new string[linestoread];
            var i = 0;
            foreach (Cheep item in cheepsRead) { // converting read data to string array, so it can be compared with the expected array.
                cheeps[i] = item.ToString();
                i++;
            }
            Assert.Equal(expected, cheeps);
        }
        #endregion






    }
}
