public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page = 1);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page = 1);
}

public class CheepService : ICheepService
{
    private readonly IDBFacade dBFacade;

    public CheepService(IDBFacade _dBFacade) => dBFacade = _dBFacade;

    public List<CheepViewModel> GetCheeps(int page)
    {
        return dBFacade.GetCheeps(page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        return dBFacade.GetCheeps(page, author);
    }
}
