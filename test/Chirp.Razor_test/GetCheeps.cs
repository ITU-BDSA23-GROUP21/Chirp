namespace Chirp.Razor_test;

using Microsoft.Extensions.DependencyInjection;
public class GetCheeps
{


    [Theory]
    [InlineData(0, null)]
    public static void GetFirstCheepOnPage(int page, string auther)
    {
        // var builder = WebApplication.CreateBuilder();
        // builder.Services.AddSingleton<IDBFacade, DBFacade>();
        // var sp = builder.BuildServiceProvider();
        // DBFacade mockFacade = sp.GetService<IDBFacade>();

        IDBFacade mockFacade = new DBFacade();
        List<CheepViewModel> cheeps = mockFacade.GetCheeps(page, auther);
        CheepViewModel firstCheep = cheeps[0];

        Assert.Equal("Helge", firstCheep.Author);
        Assert.Equal("Hello, BDSA students!", firstCheep.Message);
        Assert.Equal("08/01/23 12:16:48", firstCheep.Timestamp);

    }




    public static void GetLastCheep(string author)
    {

    }

}