static class UserInterface
{
    static public void PrintCheeps(IEnumerable<Cheep> cheeps) {
        foreach (Cheep cheep in cheeps)
        Console.WriteLine(cheep.ToString());
    }
}