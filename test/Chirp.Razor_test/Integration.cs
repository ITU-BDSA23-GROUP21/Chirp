using Xunit;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Razor;
using Chirp.Shared_test;

namespace Chirp.Razor_test;

[Collection("Environment Variable")]
public class Integration : BaseDBTest {
    public CheepService CheepServiceInit() {
        return new(new CheepRepository(_context));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async void CheepService_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        var cheepService = CheepServiceInit();
        int expectedValue = 32;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepService_GetCheeps_ZeroAndBelowParameterValues(int page) {
        //Arrange
        var cheepService = CheepServiceInit();
        int expectedCheepAmount = 32;
        CheepDto expectedFirstCheep = new("",
                                          "Jacqualine Gilcoine",
                                          "Starbuck now is what we hear the worst.",
                                          "08/01/23 13:17:39",
                                          0,
                                          null);
        CheepDto expectedLastCheep = new("",
                                         "Jacqualine Gilcoine",
                                         "With back to my friend, patience!",
                                         "08/01/23 13:16:58",
                                         0,
                                         null);

        // ExecuteCommand("Your command here");
        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheeps(page);
        int actualCheepAmount = cheeps.Count();
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();


        //Assert
        Assert.Equal(expectedCheepAmount, actualCheepAmount);
        Assert.Equal(expectedFirstCheep.Message, actualFirstCheep.Message);
        Assert.Equal(expectedFirstCheep.Author, actualFirstCheep.Author);
        Assert.Equal(expectedLastCheep.Message, actualLastCheep.Message);
        Assert.Equal(expectedLastCheep.Author, actualLastCheep.Author);

    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Roger Histand")]
    public async void CheepService_GetCheepsFromAuthor_ValidAuthorParameterValue(string author) {
        //Arrange
        var cheepService = CheepServiceInit();
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthors(new List<string>() { author }, 1);

        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public async void CheepService_GetCheepsFromAuthor_NonExistingAuthorParameterValue() {
        //Arrange 
        var cheepService = CheepServiceInit();
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthors(new List<string>() { "NonExistingAuther" }, 1);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async void CheepService_GetCheepsFromAuthor_ValidAuthorReturning32Cheeps() {
        //Arrange 
        var cheepService = CheepServiceInit();
        int expectedValue = 32;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthors(new List<string>() { "Jacqualine Gilcoine" }, 1);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepService_GetCheepsFromAuthor_ValidAuthorZeroAndBelowPageValue(int page) {
        //Arrange
        var cheepService = CheepServiceInit();
        CheepDto expectedFirstCheep = new("",
                                          "Mellie Yost",
                                          "But what was behind the barricade.",
                                          "08/01/23 13:17:33",
                                          0,
                                          null);

        CheepDto expectedLastCheep = new("",
                                          "Mellie Yost",
                                          "A well-fed, plump Huzza Porpoise will yield you about saying, sir?",
                                          "08/01/23 13:13:32",
                                          0,
                                          null);

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthors(new List<string>() { "Mellie Yost" }, page);
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();

        //Assert
        Assert.Equal(expectedFirstCheep.Message, actualFirstCheep.Message);
        Assert.Equal(expectedFirstCheep.Author, actualFirstCheep.Author);
        Assert.Equal(expectedLastCheep.Message, actualLastCheep.Message);
        Assert.Equal(expectedLastCheep.Author, actualLastCheep.Author);
    }
}
