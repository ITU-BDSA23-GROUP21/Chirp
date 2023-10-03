public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private readonly IDBFacade dBFacade;

    public CheepService(IDBFacade _dBFacade) => dBFacade = _dBFacade;

    public List<CheepViewModel> GetCheeps()
    {
        return dBFacade.GetCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return dBFacade.GetCheeps(author);
    }
}
