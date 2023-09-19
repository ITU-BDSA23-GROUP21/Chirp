static class UserInterface {
    static public void PrintCheeps(IEnumerable<Cheep> cheeps, int amount) {
        
        foreach (Cheep cheep in cheeps.Reverse().Take(amount))
            Console.WriteLine(cheep.ToString());
    }
}
