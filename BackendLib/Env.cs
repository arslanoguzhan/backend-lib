namespace BackendLib;

/// <summary>
/// A value type to hold running environment, useful for some environment specific use-cases
/// </summary>
public class Env : IEquatable<Env>
{
    public static readonly Env Production = new(
        title: "Production",
        cookieKeyPrefix: "",
        domain: "backendlib.com");

    public static readonly Env Staging = new(
        title: "Staging",
        cookieKeyPrefix: "staging",
        domain: "staging.backendlib.com");

    public static readonly Env Development = new(
        title: "Development",
        cookieKeyPrefix: "dev",
        domain: "dev.backendlib.com");

    public static readonly Env Local = new(
        title: "Local",
        cookieKeyPrefix: "local",
        domain: "backendlib.local");

    public virtual string Title { get; }

    public virtual string CookieKeyPrefix { get; }

    public virtual string Domain { get; }

    private Env(string title, string cookieKeyPrefix, string domain)
    {
        Title = title;
        CookieKeyPrefix = cookieKeyPrefix;
        Domain = domain;
    }

    public override string ToString()
    {
        return Title;
    }

    public bool Equals(Env? other)
    {
        return other is not null && Title == other.Title;
    }

    public static bool operator ==(Env? first, Env? second)
    {
        return (first is null && second is null) || (first is not null && first.Equals(second));
    }

    public static bool operator !=(Env? first, Env? second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Env);
    }

    public override int GetHashCode()
    {
        return Title.GetHashCode();
    }
}