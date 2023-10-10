using Chirp.Core;
using Chirp.Infrastructure;
using System.Collections.Generic;

namespace Chirp.Razor_test;

[Collection("Environment Variable")]
public class Integration
{
    CheepService cheepService;

    public Integration()
    {
        cheepService = new(new CheepRepository());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async void CheepService_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;
        // This test is dangerous, as it will fail when we start adding new cheeps

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepService_GetCheeps_ZeroAndBelowParameterValues(int page)
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
    public async void CheepService_GetCheepsFromAuthor_ValidAuthorParameterValue(string author)
    {
        //Arrange
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthor(author, 1);
         
        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public async void CheepService_GetCheepsFromAuthor_NonExistingAuthorParameterValue()
    {
        //Arrange 
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthor("NonExistingAuther", 1);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async void CheepService_GetCheepsFromAuthor_ValidAuthorReturning32Cheeps()
    {
        //Arrange 
        int expectedValue = 32;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepService.GetCheepsFromAuthor("Jacqualine Gilcoine", 1);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
}