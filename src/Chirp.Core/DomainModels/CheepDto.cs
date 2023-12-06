namespace Chirp.Core;

// id is now included in dto, since it avoids some expensive and imprecise operations in the repositories,
// as the frontend can now request to like a cheep by id, instead of by author / message / timestamp

// LikedByUser is true if liked, false if disliked, and null if neither
public record CheepDto(string Id, string Author, string Message, string Timestamp, int LikeCount, bool? LikedByUser);