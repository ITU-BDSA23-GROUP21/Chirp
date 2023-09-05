using System.Text.RegularExpressions;

if (args.Length == 0)
{
    Console.WriteLine("Missing arguments");
    return;
}

switch (args[0])
{
    case "read":
        //Read all Cheeps from file
        readFile();
        printCheeps();
        //Print out all Cheeps
        break;
    case "cheep":
        if (args.Length < 1) {
            Console.WriteLine("Missing cheep message");
            return;
        }
        addCheep(args[1]);
        //Write input as a new Cheep to file
        break;
    default:
        Console.WriteLine("Invalid argument");
        return;
}

void readFile()
{
    try
    {
        using var sr = new StreamReader("chirp_cli_db.csv");
        while (!sr.EndOfStream)
        {
            // Regex adapted from https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/
            var line = sr.ReadLine();
            var CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //Separating columns to array
            string[] splitLine = CSVParser.Split(line);
            // create cheep from splitLine, and add to cheep list 
        }
    }
    catch (System.Exception)
    {

        throw;
    }
}

void printCheeps() {
    // not implemented yet
}

void addCheep(string message) {
    // not implemented yet
}

//test