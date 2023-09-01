namespace BackendLib.Time;

/// <summary>
/// Default implementation of ITimeProvider
/// </summary>
public class TimeProvider : ITimeProvider
{
    public DateTimeOffset GetUtcNow()
    {
        return UtcNow;
    }

    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}