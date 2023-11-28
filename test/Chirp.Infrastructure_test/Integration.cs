using Chirp.Core;
using Chirp.Infrastructure;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Chirp.Infrastructure_test;

public class Integration : IAsyncLifetime {
    private readonly MsSqlContainer _msSqlContainer
        = new MsSqlBuilder().Build();

    public Task InitializeAsync()
        => _msSqlContainer.StartAsync();

    public Task DisposeAsync()
        => _msSqlContainer.DisposeAsync().AsTask();

    public async Task<CheepRepository> CheepRepoInit()
    {
        ChirpContext context = new(new DbContextOptionsBuilder().UseSqlServer(_msSqlContainer.GetConnectionString()).Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        return new(context);
    }

    public async Task<AuthorRepository> AuthorRepoInit() 
    {
        ChirpContext context = new(new DbContextOptionsBuilder().UseSqlServer(_msSqlContainer.GetConnectionString()).Options);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        return new(context);    
    }

    #region Cheep Repository Tests
    #region Get Cheeps Method
    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async Task CheepRepository_GetCheeps_Return32Cheeps(int page) {
        //Arrange
        CheepRepository cheepRepository = await CheepRepoInit();
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
    public async Task CheepRepository_GetCheeps_ZeroAndBelowParameterValues(int page) {
        //Arrange
        CheepRepository cheepRepository = await CheepRepoInit();
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
    public async Task CheepRepository_GetCheeps_ValidAuthorParameterValue(string author) {
        //Arrange
        CheepRepository cheepRepository = await CheepRepoInit();
        string expectedValue = author;
        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, author);

        //Assert
        Assert.All<CheepDto>(cheeps, (cheep) => { cheep.Author.Equals(expectedValue); });
    }

    [Fact]
    public async Task CheepRepository_GetCheeps_NonExistingAuthorParameterValue() {
        //Arrange
        CheepRepository cheepRepository = await CheepRepoInit();
        int expectedValue = 0;

        //Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, "NonExistingAuther");
        int actualValue = cheeps.Count();

        //Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task CheepRepository_GetCheeps_ValidAuthorReturning32Cheeps() {
        //Arrange
        CheepRepository cheepRepository = await CheepRepoInit();
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
        CheepRepository cheepRepository = await CheepRepoInit();
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
        #endregion
    #region Add Cheep Method
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient m")]
    public async Task CheepRepository_AddCheep_InvalidMessage(string message)
    {
        //Arrange 
        CheepRepository repository = await CheepRepoInit();
        string author = "Nathan Sirmon";
        string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, author, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);
        
        //Assert
        Assert.False(result.IsValid);
        Assert.Empty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidMessage()
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string author = "John Cena";
        string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, author, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.True(result.IsValid);
        Assert.NotEmpty(cheeps);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public async Task CheepRepository_AddCheep_InvalidAuthorName(string authorName)
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string email = "validemail@gmail.com";

        //Act
        ValidationResult result = await repository.AddCheep(message, authorName, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, authorName);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.False(result.IsValid);
        Assert.Empty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidNonExistingAuthor()
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string authorName = "Valid Author";
        string email = "Validemail@gmail.com";

        //Act
        ValidationResult result = await repository.AddCheep(message, authorName, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, authorName);
        CheepDto cheep = cheeps.Single();

        //Assert
        Assert.True(result.IsValid);
        Assert.Equal(authorName, cheep.Author);
        Assert.Equal(message, cheep.Message);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidExistingAuthor()
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string authorName = "Nathan Sirmon";
        string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, authorName, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, authorName);
        CheepDto cheep = cheeps.Where(c => c.Message == message).Single();

        //Assert
        Assert.True(result.IsValid);
        Assert.NotNull(cheep);
        Assert.Equal(authorName, cheep.Author);
        Assert.Equal(message, cheep.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData("InvalidEmail.com")]
    public async Task CheepRepository_AddCheep_InvalidEmail(string email)
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string author = "New Author";

        //Act
        ValidationResult result = await repository.AddCheep(message, author, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.False(result.IsValid);
        Assert.Empty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidEmail()
    {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string authorName = "valid Author";
        string email = "ValidEmail@gmail.com";
    
        //Act
        ValidationResult result = await repository.AddCheep(message, authorName, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, authorName);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.True(result.IsValid);
        Assert.NotEmpty(cheeps);
    }

    #endregion
    #endregion
    #region Author Repository Tests

    [Fact]
    public async Task AuthorRepository_GetAuthorByName_RegisteredAuthor() {
        //Arrange
        AuthorRepository authorRepository = await AuthorRepoInit();
        AuthorDto expectedAuthor = new("Helge",
                                        "ropf@itu.dk");

        //Act
        AuthorDto actualAuthor = await authorRepository.GetAuthorByName("Helge");

        //Assert
        Assert.Equal(expectedAuthor, actualAuthor);
    }

    [Fact]
    public async Task AuthorRepository_GetAuthorByEmail_RegisteredAuthor() {
        //Arrange
        AuthorRepository authorRepository = await AuthorRepoInit();
        AuthorDto expectedAuthor = new("Helge",
                                        "ropf@itu.dk");

        //Act
        AuthorDto actualAuthor = await authorRepository.GetAuthorByEmail("ropf@itu.dk");

        //Assert
        Assert.Equal(expectedAuthor, actualAuthor);
    }

    #endregion
}
