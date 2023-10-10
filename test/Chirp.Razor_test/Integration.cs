using Chirp.Core;
using Chirp.Infrastructure;
using System.Collections.Generic;

namespace Chirp.Razor_test;

[Collection("Environment Variable")]
public class Integration
{
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public static async void CheepService_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;
        // This test is dangerous, as it will fail when we start adding new cheeps
        CheepService cheepService = new(new CheepRepository());

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public static async void CheepService_GetCheeps_ZeroAndBelowParameterValues(int page)
    {
        //Arrange
        int expectedCheepAmount = 32;
        CheepDto expectedFirstCheep = new("Jacqualine Gilcoine", 
                                          "Starbuck now is what we hear the worst.", 
                                          "08/01/23 13:17:39");
        CheepDto expectedLastCheep =  new("Jacqualine Gilcoine",
                                          "With back to my friend, patience!",
                                          "08/01/23 13:16:58");

        // ExecuteCommand("Your command here");
        CheepService cheepService = new(new CheepRepository());
        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheeps(page);
        int      actualCheepAmount = cheeps.Count();
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();


        //Assert
        Assert.Equal(expectedCheepAmount, actualCheepAmount);
        Assert.Equal(expectedFirstCheep, actualFirstCheep);
        Assert.Equal(expectedLastCheep, actualLastCheep);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Roger Histand")]
    public static async void CheepService_GetCheepsFromAuthor_ValidAuthorParameterValue(string author)
    {
        //Arrange
        CheepService cheepService = new(new CheepRepository());
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthor(author, 1);
         
        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public static async void CheepService_GetCheepsFromAuthor_NonExistingAuthorParameterValue()
    {
        //Arrange 
        CheepService cheepService = new(new CheepRepository());
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthor("NonExistingAuther", 1);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
}