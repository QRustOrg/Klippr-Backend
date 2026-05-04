namespace Application.OutboundServices.Hashing;

public interface IHashingService
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
