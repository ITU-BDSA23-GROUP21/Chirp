namespace Chirp.Core;

// Id is included in dto, since it avoids some unnecessary complexity in the repositories
// as the frontend can now request to like a cheep by id, instead of by (author, message, timestamp)

// LikedByUser is true if liked, false if disliked, and null if neither
/// <summary>
/// Stores and transfers the necessary data of a cheep from the database to the frontend.
/// </summary>
/// <param name="Id">Id of the cheep.</param>
/// <param name="Author">Name of the author that this cheep is written by.</param>
/// <param name="Message">The context of the cheep.</param>
/// <param name="Timestamp">The time which the cheep was made.</param>
/// <param name="LikeCount">Combination of likes and dislikes the cheep has, expresed in a single number.</param>
/// <param name="LikedByUser">Used to check if the cheep is liked by the authorized user. Is true if liked, false if disliked, and null if neither.</param>
public record CheepDto(string Id, string Author, string Message, string Timestamp, int LikeCount, bool? LikedByUser);