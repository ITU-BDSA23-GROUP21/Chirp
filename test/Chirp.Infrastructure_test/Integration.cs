using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Infrastructure_test;

[Collection("Environment Variable")]
public class Integration
{
    CheepRepository cheepRepository;
    public Integration()
    {
        cheepRepository = new CheepRepository();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async void CheepRepository_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;
        // This test is dangerous, as it will fail when we start adding new cheeps

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepRepository_GetCheeps_ZeroAndBelowParameterValues(int page)
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
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page);
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
    public async void CheepRepository_GetCheeps_ValidAuthorParameterValue(string author)
    {
        //Arrange
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, author);
         
        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public async void CheepRepository_GetCheeps_NonExistingAuthorParameterValue()
    {
        //Arrange 
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, "NonExistingAuther");
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async void CheepRepository_GetCheeps_ValidAuthorReturning32Cheeps()
    {
        //Arrange 
        int expectedValue = 32;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, "Jacqualine Gilcoine");
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepRepository_GetCheeps_ValidAuthorZeroAndBelowPageValue(int page)
    {
        //Arrange
        CheepDto expectedFirstCheep = new("Jacqualine Gilcoine", 
                                          "Starbuck now is what we hear the worst.",
                                          "08/01/23 13:17:39");
                                        
        CheepDto expectedLastCheep =  new("Jacqualine Gilcoine",
                                          "Now, amid the cloud-scud.",
                                          "08/01/23 13:16:30");
        
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page, "Jacqualine Gilcoine");
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();
        
        //Assert
        Assert.Equal(expectedFirstCheep, actualFirstCheep);
        Assert.Equal(expectedLastCheep, actualLastCheep);
    }
}