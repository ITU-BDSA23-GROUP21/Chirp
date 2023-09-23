public static class UserInterface {
    public static void PrintCheeps(List<Cheep> cheeps) {
        foreach (Cheep cheep in cheeps)
            Console.WriteLine(cheep.ToString());
    }
}
