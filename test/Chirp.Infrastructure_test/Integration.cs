using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Shared_test;
using FluentValidation.Results;

namespace Chirp.Infrastructure_test;

public class Integration : BaseDBTest {
    public CheepRepository CheepRepoInit() {
        return new(_context);
    }

    public AuthorRepository AuthorRepoInit() {
        return new(_context);
    }

    #region Cheep Repository Tests
    #region Add Cheep Method
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient m")]
    public async Task CheepRepository_AddCheep_InvalidMessage(string message) {
        //Arrange 
        CheepRepository repository = CheepRepoInit();
        string author = "Nathan Sirmon";
        // string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, author);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.False(result.IsValid);
        Assert.Empty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidMessage() {
        //Arrange
        CheepRepository repository = CheepRepoInit();
        string message = "Valid Message";
        string author = "Nathan Sirmon";
        // string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, author);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1, author);
        cheeps = cheeps.Where(c => c.Message == message);

        //Assert
        Assert.True(result.IsValid);
        Assert.NotEmpty(cheeps);
    }

    [Fact]
    public async Task CheepRepository_AddCheep_ValidExistingAuthor() {
        //Arrange
        CheepRepository repository = CheepRepoInit();
        string message = "Valid Message";
        string authorName = "Nathan Sirmon";
        // string email = "Nathan+Sirmon@dtu.dk";

        //Act
        ValidationResult result = await repository.AddCheep(message, authorName);
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
    [InlineData("   ")]
    [InlineData("NonExistingAuthor")]
    public async Task CheepRepository_AddCheep_InvalidEmailOrNonExistingAuthor(string author) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        string message = "Valid Message";
        // string email = "ValidEmail@mail.com";

        // Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.AddCheep(message, author));
    }

    #endregion
    #region Like Cheep Method
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CheepRepository_LikeCheep_ValidUserEmailValidCheepId(bool value) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        string email = "Octavio.Wagganer@dtu.dk";
        IEnumerable<CheepDto> arrangecheeps = await repository.GetCheeps(1);
        string cheepId = arrangecheeps.First().Id;
        int expectedLikeValue = value ? 1 : -1;

        // Act
        await repository.LikeCheep(email, cheepId, value);
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1);
        int actualLikeValue = cheeps.First().LikeCount;

        // Assert
        Assert.Equal(expectedLikeValue, actualLikeValue);
    }

    #endregion
    #region Remove Like Method
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CheepRepository_RemoveLike_ValidUserEmailValidCheepId(bool likeValue) {
        // Arrange
        CheepRepository repository = CheepRepoInit();
        string userEmail = "Octavio.Wagganer@dtu.dk";
        IEnumerable<CheepDto> cheeps = await repository.GetCheeps(1);
        string cheepId = cheeps.First().Id;
        int expectedBeforeRemoveLikeValue = likeValue ? 1 : -1;
        int expectedAfterRemoveLikeValue = 0;

        // Act
        await repository.LikeCheep(userEmail, cheepId, likeValue);
        cheeps = await repository.GetCheeps(1);
        int actualBeforeRemoveLikeValue = cheeps.First().LikeCount;
        await repository.RemoveLike(userEmail, cheepId);
        cheeps = await repository.GetCheeps(1);
        int actualAfterRemoveLikeValue = cheeps.First().LikeCount;

        // Assert
        Assert.Equal(expectedBeforeRemoveLikeValue, actualBeforeRemoveLikeValue);
        Assert.Equal(expectedAfterRemoveLikeValue, actualAfterRemoveLikeValue);
    }

    #endregion
    #endregion
    #region Author Repository Tests
    #region Create Author Method
    [Fact]
    public async Task AuthorRepository_CreateAuthor_ValidEmail() {
        //Arrange
        var repository = AuthorRepoInit();
        string authorName = "valid Author";
        string email = "ValidEmail@gmail.com";

        //Act
        await repository.CreateAuthor(new AuthorDto(authorName, email));
        var author = await repository.GetAuthorByName(authorName);

        //Assert
        Assert.Equal(authorName, author.Name);
        Assert.Equal(email, author.Email);
    }

    [Fact]
    public async Task AuthorRepository_CreateAuthor_ValidNonExistingAuthor() {
        //Arrange
        var repository = AuthorRepoInit();
        string authorName = "Valid Author";
        string email = "Validemail@gmail.com";

        //Act
        await repository.CreateAuthor(new AuthorDto(authorName, email));
        var author = await repository.GetAuthorByName(authorName);

        //Assert
        Assert.Equal(authorName, author.Name);
        Assert.Equal(email, author.Email);
    }
    #endregion
    #region Follow Method
    //Is this too complex for an integration test?
    [Fact]
    public async Task AuthorRepository_Follow_ValidUsers() {
        //Arrange
        AuthorRepository authorRepository = AuthorRepoInit();
        AuthorDto expectedFollower = await authorRepository.GetAuthorByName("Rasmus");

        //Act
        await authorRepository.Follow("Helge", "Rasmus");
        IEnumerable<AuthorDto> followings = await authorRepository.GetFollowings("Helge");
        AuthorDto actualFollower = followings.First();

        //Assert
        Assert.Equal(expectedFollower, actualFollower);
    }
    [Theory]
    [InlineData("", 0)]
    [InlineData("   ", 0)]
    [InlineData("NonExistingName", 0)]
    [InlineData("", 1)]
    [InlineData("   ", 1)]
    [InlineData("NonExistingName", 1)]
    public async Task AuthorRepository_Follow_InvalidUsers(string name, int parameter) {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();
        string validName = "Rasmus";

        //Act / Assert
        if (parameter == 0) {
            await Assert.ThrowsAnyAsync<Exception>(async () => await repository.Follow(name, validName));
        }
        else {
            await Assert.ThrowsAnyAsync<Exception>(async () => await repository.Follow(validName, name));
        }
    }

    #endregion
    #region Unfollow Method
    [Fact]
    public async Task AuthorRepository_Unfollow_ValidUsers() {
        //Arrange
        AuthorRepository authorRepository = AuthorRepoInit();
        AuthorDto expectedTempFollower = await authorRepository.GetAuthorByName("Rasmus");

        //Act
        await authorRepository.Follow("Helge", "Rasmus");
        //Temp followings are used to ensure that the user was actually followed
        IEnumerable<AuthorDto> tempFollowings = await authorRepository.GetFollowings("Helge");
        AuthorDto actualTempFollower = tempFollowings.First();

        await authorRepository.UnFollow("Helge", "Rasmus");
        IEnumerable<AuthorDto> actualFollowings = await authorRepository.GetFollowings("Helge");

        //Assert
        Assert.Equal(expectedTempFollower, actualTempFollower);
        Assert.Empty(actualFollowings);
    }
    [Theory]
    [InlineData("", 0)]
    [InlineData("   ", 0)]
    [InlineData("NonExistingName", 0)]
    [InlineData("", 1)]
    [InlineData("   ", 1)]
    [InlineData("NonExistingName", 1)]
    public async Task AuthorRepository_UnFollow_InvalidUsers(string name, int parameter) {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();
        string validName = "Rasmus";

        //Act / Assert
        if (parameter == 0) {
            await Assert.ThrowsAnyAsync<Exception>(async () => await repository.UnFollow(name, validName));
        }
        else {
            await Assert.ThrowsAnyAsync<Exception>(async () => await repository.UnFollow(validName, name));
        }
    }
    #endregion
    #region Get Followings Method
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("InavlidAuthorName")]
    public async Task AuthorRepository_GetFollowings_InvalidAuthorNames(string authorName) {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();

        //Act
        IEnumerable<AuthorDto> authors = await repository.GetFollowings(authorName);

        // Assert
        Assert.Empty(authors);
    }
    #endregion
    #region Anonymize Method
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("InvalidAuthor")]
    public async Task AuthorRepository_Anonymize_InvalidOrNonexistingAuthorName(string authorName) {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();

        //Act / Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await repository.Anonymize(authorName));
    }
    [Fact]
    public async Task AuthorRepository_Anonymize_ValidAuthorName() {
        // Arrange
        AuthorRepository repository = AuthorRepoInit();
        CheepRepository cheepRepository = CheepRepoInit();
        string expectedBeforeAuthor = "Jacqualine Gilcoine";
        int expectedAfterCheepCount = 0;

        // Act
        IEnumerable<CheepDto> cheeps = await cheepRepository.GetCheeps(1);
        string actualBeforeAuthor = cheeps.First().Author;
        await repository.Anonymize("Jacqualine Gilcoine");
        cheeps = await cheepRepository.GetCheeps(1);
        string actualAfterAuthor = cheeps.First().Author;
        cheeps = await cheepRepository.GetCheeps(1, "Jacqualine Gilcoine");
        int actualAfterCheepCount = cheeps.Count();

        // Assert
        Assert.Equal(expectedBeforeAuthor, actualBeforeAuthor);
        Assert.NotEqual(expectedBeforeAuthor, actualAfterAuthor);
        Assert.Equal(expectedAfterCheepCount, actualAfterCheepCount);
    }
    #endregion
    #endregion
}
