﻿namespace Chirp.Shared;
public record Cheep(string Author, string Message, long Timestamp) {
    private DateTimeOffset LongToLocalTime() {
        return DateTimeOffset.FromUnixTimeSeconds(this.Timestamp).ToLocalTime();
    }

    public override string ToString() {
        return $"{Author} @ {LongToLocalTime():dd/mm/yy HH:mm:ss}: {Message}";
    }
}