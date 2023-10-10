using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Razor_test;
public class Integration
{
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public static void CheepService_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;
        // This test is dangerous, as it will fail when we start adding new cheeps
        Environment.SetEnvironmentVariable("CHIRPDBPATH", "./TestChirp.db");
        CheepService cheepService = new(new CheepRepository());

        //Act
        IEnumerable<CheepDto> cheeps = cheepService.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public static void CheepService_GetCheeps_ZeroAndBelowParameterValues(int page)
    {
        //Arrange
        int expectedCheepAmount = 32;
        CheepDto expectedFirstCheep = new("Helge", "Hello", );
        CheepDto expectedLastCheep;


        //Act

    }
}