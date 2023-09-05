using System.Text.RegularExpressions;

if (args.Length == 0)
{
    Console.WriteLine("Missing arguments");
    return;
}

switch (args[0])
{
    case "read":
        var cheeps = readFile();
        printCheeps(cheeps);
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

List<Cheep> readFile()
{
    List<Cheep> cheeps = new List<Cheep>();
    try
    {
        using var sr = new StreamReader("chirp_cli_db.csv");
        sr.ReadLine(); // Skip first line. CSV file format is hardcoded in fileReader
        while (!sr.EndOfStream)
        {
            // Regex adapted from https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/
            var line = sr.ReadLine();
            var CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            // Separating columns to array
            // TODO: ""s should not be included in message
            string[] splitLine = CSVParser.Split(line);
            // Create cheep from splitLine, and add to cheep list 
            cheeps.Add(new Cheep(splitLine[0], splitLine[1], splitLine[2]));
        }
        return cheeps;
    }
    catch (System.Exception)
    {
        throw;
    }
}

void printCheeps(List<Cheep> cheeps) {
    foreach (Cheep cheep in cheeps)
        Console.WriteLine(cheep.ToString());
}

void addCheep(string message) {
    // not implemented yet
}

//test 3