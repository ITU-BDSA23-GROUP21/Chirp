namespace Chirp.Core;
public record CheepDto(string Author, string Message, string Timestamp, int LikeCount, bool LikedByUser);