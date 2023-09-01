namespace BackendLib.Cookies;

/// <summary>
/// Mockable cookie access.
/// </summary>
/// <typeparam name="TCookie"></typeparam>
public interface ICookieService<TCookie>
    where TCookie : ICookie
{
    string Read();

    void Write(string value);

    void Delete();
}