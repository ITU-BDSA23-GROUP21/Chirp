static class UserInterface
{
    static public void printCheeps(IEnumerable<Cheep> cheeps) {
        foreach (Cheep cheep in cheeps)
        Console.WriteLine(cheep.ToString());
    }
}