using SimpleDB;
using Xunit;

namespace SimpleDB_test {

    public class TestRead {

        //test if read method reads the expected amount of lines from file
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(0)]

        public void ReadGivenAmoutOfLinesTest(int linesToRead) {
            IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>("../../../../../data/chirp_cli_db.csv");
            var cheepsRead = database.Read(linesToRead);

            Assert.Equal(linesToRead, cheepsRead.Count());
        }

        //test if the read method returns what is expected

        [Theory]
        [InlineData(1, new string[] { "ropf @ 01/09/23 14:09:20: Hello, BDSA students!" })]
        [InlineData(2, new string[] { "ropf @ 01/09/23 14:09:20: Hello, BDSA students!", "rnie @ 02/19/23 14:19:38: Welcome to the course!" })]

        public void ReadTest(int linestoread, string[] expected) {
            IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>("../../../../../data/chirp_cli_db.csv");
            var cheepsRead = database.Read(linestoread);
            string[] cheeps = new string[linestoread];
            var i = 0;
            foreach (Cheep item in cheepsRead) // converting read data to string array, so it can be compared with the expected array.
            {
                cheeps[i] = item.ToString();
                i++;
            }

            Assert.Equal(expected, cheeps);
        }
    }
}
