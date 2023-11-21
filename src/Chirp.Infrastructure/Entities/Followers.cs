namespace Chirp.Infrastructure;
public class Followers {
    public required Author Follower { get; set; }
    public required Author Following { get; set; }

}