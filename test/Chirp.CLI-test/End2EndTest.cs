using System;
using System.Diagnostics;
using System.ComponentModel;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace Chirp.CLI_test
{
    public class End2EndTest
    {
        const string START_OF_HELP = "Description:\n";

        private static string LaunchProgram(string arg)
        {
            string output = "";
            using (var process = new Process())
            {
                process.StartInfo.FileName = "/usr/bin/dotnet";
                process.StartInfo.Arguments = $"./Chirp.CLI.dll {arg}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI/bin/Debug/net7.0/";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                output = reader.ReadToEnd();
                process.WaitForExit();
            }
            return output;
        }

        [Fact]
        static void NoCommand()
        {
            //Arrange
            string command = "";
            string expectedOutput = START_OF_HELP;
            //Act
            string output = LaunchProgram(command);
            //Assert
            Assert.Matches(expectedOutput, output);
        }

        [Fact]
        static void InvalidCommand()
        {
            //Arrange
            string command = "Test";
            string expectedOutput = START_OF_HELP;
            //Act
            string output = LaunchProgram(command);
            //Assert
            Assert.Contains(expectedOutput, output);
        }

        [Theory]
        [InlineData("")] //data to be tested in genericTest() *the cheep message*
        [InlineData("2")]
        static void ReadCommand_ValidArguments(string args)
        {
            // Arrange
            string command = "read ";
            string expectedOutputRegex = @"\S+ @ (\d\d/){2}\d\d (\d\d:){3} .+";
            // Act
            string output = LaunchProgram(command + args);
            string[] cheeps = output.Trim().Split("\n");
            // Assert
            foreach(string cheep in cheeps)
                Assert.Matches(expectedOutputRegex, cheep);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("3 4")]
        [InlineData("3 test")]
        static void ReadCommand_InvalidArgumentType(string args)
        {
            //Arrange
            string command = "read ";
            string expectedOutput = START_OF_HELP;
            //Act
            string output = LaunchProgram(command + args);
            //Assert
            Assert.StartsWith(expectedOutput, output);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("45")]
        [InlineData("\"Hello World!\"")]
        static void CheepCommand_OneArgument(string args)
        {
            //Arrange
            string command = "cheep ";
            string expectedOutput = "";
            //Act
            string output = LaunchProgram(command + args);
            //Assert
            Assert.Equal(expectedOutput, output);
        }

        [Theory]
        [InlineData("Test Test")]
        [InlineData("Test 3")]
        static void CheepCommand_TwoArguments(string args)
        {
            //Arrange
            string command = "cheep ";
            string expectedOutput = START_OF_HELP;
            //Act
            string output = LaunchProgram(command + args);
            //Assert
            Assert.StartsWith(expectedOutput, output);
        }

        [Fact]
        static void CheepCommand_ZeroArguments()
        {
            //Arrange
            string command = "cheep ";
            string expectedOutput = START_OF_HELP;
            //Act
            string output = LaunchProgram(command);
            //Assert
            Assert.StartsWith(expectedOutput, output);
        }
    }
}
