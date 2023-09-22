public static class UserInterface {
    public static async void PrintCheeps(List<Cheep> cheeps) {
        foreach (Cheep cheep in cheeps)
            Console.WriteLine(cheep.ToString());
    }
}
