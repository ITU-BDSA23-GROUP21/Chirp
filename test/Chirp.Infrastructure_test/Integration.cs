using Chirp.Core;
using Chirp.Infrastructure;
using DotNet.Testcontainers.Containers;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Chirp.Infrastructure_test;

public class Integration : IAsyncLifetime {
    private readonly DockerContainer _container
        = Environment.GetEnvironmentVariable("SERVER") == "POSTGRES" ? new PostgreSqlBuilder().Build() : new MsSqlBuilder().Build();

    public Task InitializeAsync()
        => _container.StartAsync();

    public Task DisposeAsync()
        => _container.DisposeAsync().AsTask();

    public static ChirpContext GetContext(IDatabaseContainer? container)
        => Environment.GetEnvironmentVariable("SERVER") == "POSTGRES" ?
            new(new DbContextOptionsBuilder().UseNpgsql(container?.GetConnectionString()).Options) :
            new(new DbContextOptionsBuilder().UseSqlServer(container?.GetConnectionString()).Options);

    public async Task<CheepRepository> CheepRepoInit() {
        // Casting as a quick fix for type issues. Should always be safe, as _container is readonly and only instantiated as a database container
        var container = _container as IDatabaseContainer;
        ChirpContext context = GetContext(container);
        await context.Database.EnsureCreatedAsync();
        DbInitializer.SeedDatabase(context);
        return new(context);
    }

    public async Task<AuthorRepository> AuthorRepoInit() {
        var container = _container as IDatabaseContainer;
        ChirpContext context = GetContext(container);
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
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(page);
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
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1, "NonExistingAuthor");
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
    #region Add Cheep Method
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient m")]
    public async Task CheepRepository_AddCheep_InvalidMessage(string message) {
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
    public async Task CheepRepository_AddCheep_ValidMessage() {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        string message = "Valid Message";
        string author = "Nathan Sirmon";
        string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, author, email);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.True(result.IsValid);
        Assert.NotEmpty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidExistingAuthor() {
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

    #endregion
    #region Like Cheep Method
    [Theory]
    [InlineData("")]
    [InlineData("                 ")]
    [InlineData("invalidemail.com")]
    public async Task CheepRepository_LikeCheep_InvalidUserEmail(string email) {
        //Arrange
        CheepRepository repository = await CheepRepoInit();
        IEnumerable<CheepDto> cheepsbefore = await repository.GetCheeps(1);
        string cheepId = cheepsbefore.First().Id;

        //Act
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.LikeCheep(email, cheepId, true));
    }

    #endregion
    #endregion
    #region Author Repository Tests

    [Theory]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData("InvalidEmail.com")]
    public async Task AuthorRepository_CreateAuthor_InvalidEmail(string email) {
        //Arrange
        var repository = await AuthorRepoInit();
        string author = "New Author";

        // Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.CreateAuthor(new AuthorDto(author, email)));
    }

    [Fact]
    public async Task AuthorRepository_CreateAuthor_ValidEmail() {
        //Arrange
        var repository = await AuthorRepoInit();
        string authorName = "valid Author";
        string email = "ValidEmail@gmail.com";

        //Act
        await repository.CreateAuthor(new AuthorDto(authorName, email));
        var author = await repository.GetAuthorByName(authorName);

        //Assert
        Assert.Equal(authorName, author.Name);
        Assert.Equal(email, author.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    public async Task AuthorRepository_CreateAuthor_InvalidAuthorName(string authorName) {
        //Arrange
        var repository = await AuthorRepoInit();
        string email = "validemail@gmail.com";

        //Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.CreateAuthor(new AuthorDto(authorName, email)));
    }

    [Fact]
    public async Task AuthorRepository_CreateAuthor_ValidNonExistingAuthor() {
        //Arrange
        var repository = await AuthorRepoInit();
        string authorName = "Valid Author";
        string email = "Validemail@gmail.com";

        //Act
        await repository.CreateAuthor(new AuthorDto(authorName, email));
        var author = await repository.GetAuthorByName(authorName);

        //Assert
        Assert.Equal(authorName, author.Name);
        Assert.Equal(email, author.Email);
    }

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

    //Is this too complex for an integration test?
    [Fact]
    public async Task AuthorRepository_Follow_ValidUsers() {
        //Arrange
        AuthorRepository authorRepository = await AuthorRepoInit();
        AuthorDto expectedFollower = await authorRepository.GetAuthorByName("Rasmus");

        //Act
        await authorRepository.Follow("Helge", "Rasmus");
        IEnumerable<AuthorDto> followings = await authorRepository.GetFollowings("Helge", "ropf@itu.dk");
        AuthorDto actualFollower = followings.First();

        //Assert
        Assert.Equal(expectedFollower, actualFollower);
    }

    [Fact]
    public async Task AuthorRepository_Unfollow_ValidUsers() {
        //Arrange
        AuthorRepository authorRepository = await AuthorRepoInit();
        AuthorDto expectedTempFollower = await authorRepository.GetAuthorByName("Rasmus");

        //Act
        await authorRepository.Follow("Helge", "Rasmus");
        //Temp followings are used to ensure that the user was actually followed
        IEnumerable<AuthorDto> tempFollowings = await authorRepository.GetFollowings("Helge", "ropf@itu.dk");
        AuthorDto actualTempFollower = tempFollowings.First();

        await authorRepository.UnFollow("Helge", "Rasmus");
        IEnumerable<AuthorDto> actualFollowings = await authorRepository.GetFollowings("Helge", "ropf@itu.dk");

        //Assert
        Assert.Equal(expectedTempFollower, actualTempFollower);
        Assert.Empty(actualFollowings);
    }

    #endregion
}
