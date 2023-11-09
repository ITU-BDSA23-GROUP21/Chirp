using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Shared_test;

namespace Chirp.Infrastructure_test;

[Collection("Environment Variable")]
public class Integration : BaseDBTest {
    readonly CheepRepository cheepRepository;
    readonly AuthorRepository authorRepository;
    public Integration() {
        cheepRepository = new CheepRepository(CreateContext());
        authorRepository = new AuthorRepository(CreateContext());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async void CheepRepository_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        int expectedValue = 32;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async void CheepRepository_GetCheeps_ZeroAndBelowParameterValues(int page) {
        //Arrange
        int expectedCheepAmount = 32;
        CheepDto expectedFirstCheep = new("Jacqualine Gilcoine",
                                          "Starbuck now is what we hear the worst.",
                                          "08/01/23 13:17:39");
        CheepDto expectedLastCheep = new("Jacqualine Gilcoine",
                                          "With back to my friend, patience!",
                                          "08/01/23 13:16:58");

        // ExecuteCommand("Your command here");
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page);
        int actualCheepAmount = cheeps.Count();
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
    public async void CheepRepository_GetCheeps_ValidAuthorParameterValue(string author) {
        //Arrange
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, author);

        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public async void CheepRepository_GetCheeps_NonExistingAuthorParameterValue() {
        //Arrange 
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, "NonExistingAuther");
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async void CheepRepository_GetCheeps_ValidAuthorReturning32Cheeps() {
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
    public async void CheepRepository_GetCheeps_ValidAuthorZeroAndBelowPageValue(int page) {
        //Arrange
        CheepDto expectedFirstCheep = new("Mellie Yost",
                                          "But what was behind the barricade.",
                                          "08/01/23 13:17:33");

        CheepDto expectedLastCheep = new("Mellie Yost",
                                          "A well-fed, plump Huzza Porpoise will yield you about saying, sir?",
                                          "08/01/23 13:13:32");

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page, "Mellie Yost");
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();

        //Assert
        Assert.Equal(expectedFirstCheep, actualFirstCheep);
        Assert.Equal(expectedLastCheep, actualLastCheep);
    }

    [Fact]
    public async void AuthorRepository_GetAuthorByName_RegisteredAuthor() {
        //Arrange
        AuthorDto expectedAuthor = new("Helge",
                                        "ropf@itu.dk");

        //Act
        AuthorDto actualAuthor = await authorRepository.GetAuthorByName("Helge");

        //Assert
        Assert.Equal(expectedAuthor, actualAuthor);
    }

    [Fact]
    public async void AuthorRepository_GetAuthorByEmail_RegisteredAuthor() {
        //Arrange
        AuthorDto expectedAuthor = new("Helge",
                                        "ropf@itu.dk");

        //Act
        AuthorDto actualAuthor = await authorRepository.GetAuthorByEmail("ropf@itu.dk");

        //Assert
        Assert.Equal(expectedAuthor, actualAuthor);
    }

    //TODO: Finish test when method is done
    [Fact]
    public async void AuthorRepository_CreateAuthor() {
        //Arrange


        //Act


        //Assert
    }
}
