using Chirp.Core;
using Chirp.Infrastructure;
using System.Collections.Generic;

namespace Chirp.Razor_test;
public class Integration
{
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public static async void CheepService_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;
        // This test is dangerous, as it will fail when we start adding new cheeps
        Environment.SetEnvironmentVariable("CHIRPDBPATH", "./TestChirp.db");
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
        CheepDto expectedFirstCheep = new("Helge", "Hello", "08/");
        CheepDto expectedLastCheep = new("", "", "");

        Environment.SetEnvironmentVariable("CHIRPDBPATH", "./TestChirp.db");
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
}