namespace Chirp.Razor_test;

using Microsoft.Extensions.DependencyInjection;
public class Unit {


    [Theory]
    [InlineData(0, null)]
    public static void GetFirstCheepOnPage(int page, string auther) {
        // This test is dangerous, as it will fail when we start adding new cheeps

        IDBFacade mockFacade = new DBFacade();
        List<CheepViewModel> cheeps = mockFacade.GetCheeps(page, auther);
        CheepViewModel firstCheep = cheeps[0];

        Assert.Equal("Jacqualine Gilcoine", firstCheep.Author);
        Assert.Equal("Starbuck now is what we hear the worst.", firstCheep.Message);
        Assert.Equal("08/01/23 13:17:39", firstCheep.Timestamp);

    }

    [Theory]
    public static void GetLastCheep(string author) {

    }

}
