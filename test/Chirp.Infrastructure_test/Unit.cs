using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Shared_test;

namespace Chirp.Infrastructure_test;

public class Unit : BaseDBTest {
    public CheepRepository CheepRepoInit() {
        return new(_context);
    }

    public AuthorRepository AuthorRepoInit() {
        return new(_context);
    }
    
    #region Get Cheeps Without Author Method
    [Fact]
    public async Task CheepRepository_GetCheepsNoAuthor_PositivePageNr() {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        int pagenr = 2;
        int expectedCheepCount = 32;

        // Act
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(pagenr);
        int actualCheepCount = cheeps.Count();

        // Assert
        Assert.Equal(expectedCheepCount, actualCheepCount);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task CheepRepository_GetCheepsNoAuthor_NegativeAndZeroPageNr(int pagenr) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        int expectedCheepCount = 32;
        string expectedFirstCheep = "Starbuck now is what we hear the worst.";
        string expectedLastCheep = "With back to my friend, patience!";

        // Act
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(pagenr);
        int actualCheepCount = cheeps.Count();
        string actualFirstCheep = cheeps.First().Message;
        string actualLastCheep = cheeps.Last().Message;

        // Assert
        Assert.Equal(expectedCheepCount, actualCheepCount);
        Assert.Equal(expectedFirstCheep, actualFirstCheep);
        Assert.Equal(expectedLastCheep, actualLastCheep); ;
    }
    #endregion
    #region Get Cheeps One Author Method
    [Theory]
    [InlineData("Helge")]
    [InlineData("Roger Histand")]
    public async Task CheepRepository_GetCheeps_ValidAuthorParameterValue(string author) {
        //Arrange
        CheepRepository cheepRepository = CheepRepoInit();
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, author);

        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("NonExistingAuthor")]
    public async Task CheepRepository_GetCheeps_InvalidAuthor(string authorName) {
        //Arrange
        CheepRepository cheepRepository = CheepRepoInit();
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, authorName);
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task CheepRepository_GetCheeps_ValidAuthorReturning32Cheeps() {
        //Arrange
        CheepRepository cheepRepository = CheepRepoInit();
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
    public async Task CheepRepository_GetCheeps_ValidAuthorZeroAndBelowPageValue(int page) {
        //Arrange
        CheepRepository cheepRepository = CheepRepoInit();
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
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page, "Mellie Yost");
        CheepDto actualFirstCheep = cheeps.First();
        CheepDto actualLastCheep = cheeps.Last();

        //Assert
        Assert.Equal(expectedFirstCheep.Message, actualFirstCheep.Message);
        Assert.Equal(expectedFirstCheep.Author, actualFirstCheep.Author);
        Assert.Equal(expectedLastCheep.Message, actualLastCheep.Message);
        Assert.Equal(expectedLastCheep.Author, actualLastCheep.Author);
    }
    #endregion

    [Theory]
    [InlineData("")]
    [InlineData("                 ")]
    [InlineData("invalidemail.com")]
    public async Task CheepRepository_LikeCheep_InvalidUserEmail(string email) {
        //Arrange
        CheepRepository repository = CheepRepoInit();
        IEnumerable<CheepDto> cheepsbefore = await repository.GetCheeps(1);
        string cheepId = cheepsbefore.First().Id;

        //Act
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.LikeCheep(email, cheepId, true));
    }
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("23r23ffgws3bvb5k8954")]
    public async Task CheepRepository_LikeCheep_InvalidCheepId(string id) {
        // Arrange 
        CheepRepository repository = CheepRepoInit();
        string email = "Jacqualine.Gilcoine@gmail.com";
        bool value = true;

        // Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.LikeCheep(email, id, value));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalidemail")]
    [InlineData("Nonexisting@email.com")]
    public async Task CheepRepository_RemoveLike_InvalidUserEmail(string userEmail) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1);
        string cheepId = cheeps.First().Id;

        // Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.RemoveLike(userEmail, cheepId));
    }
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ikbr3b33k3qdfkq2")]
    public async Task CheepRepository_RemoveLike_InvalidCheepId(string cheepId) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        string userEmail = "Jacqualine.Gilcoine@gmail.com";

        // Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.RemoveLike(userEmail, cheepId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public async Task AuthorRepository_CreateAuthor_InvalidAuthorName(string authorName) {
        //Arrange
        var repository = AuthorRepoInit();
        string email = "validemail@gmail.com";

        //Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.CreateAuthor(new AuthorDto(authorName, email)));
    }

    [Theory]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData("InvalidEmail.com")]
    public async Task AuthorRepository_CreateAuthor_InvalidEmail(string email) {
        //Arrange
        var repository = AuthorRepoInit();
        string author = "New Author";

        // Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.CreateAuthor(new AuthorDto(author, email)));
    }

    #region Get Author By Name Method
    [Fact]
    public async Task AuthorRepository_GetAuthorByName_RegisteredAuthor() {
        //Arrange
        AuthorRepository authorRepository = AuthorRepoInit();
        AuthorDto expectedAuthor = new("Helge",
                                        "ropf@itu.dk");

        //Act
        AuthorDto actualAuthor = await authorRepository.GetAuthorByName("Helge");

        //Assert
        Assert.Equal(expectedAuthor, actualAuthor);
    }
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("NonexistingAuthor")]
    public async Task AuthorRepository_GetAuthorByName_InvalidOrNonExsistingAuthor(string authorName) {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();

        // Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.GetAuthorByName(authorName));
    }
    #endregion
}