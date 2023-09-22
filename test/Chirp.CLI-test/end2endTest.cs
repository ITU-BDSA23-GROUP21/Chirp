using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Chirp.CLI_test
{
    public class end2end
    {
        #region end2end   // Arrange
        [Theory]
        [InlineData("")] //data to be tested in genericTest() *the cheep message*
        static void read_cheep_10(string str)
        {
            string output = "";
            var csvDB = Program.setDB();
            csvDB.Store(new Cheep("ropf", "Hello, World!", 000));
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = "./src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.dll read";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            string fstCheep = output.Split("\n")[0];
            Program.PrintCheeps(5);
            // Assert
            Assert.StartsWith("ropf", fstCheep);
            Assert.EndsWith("Hello, World!", fstCheep);
        }
        #endregion


    }
}
