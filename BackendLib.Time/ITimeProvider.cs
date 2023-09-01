namespace BackendLib.Time;

/// <summary>
/// Time provider to make it possible to mock time related methods, in unit tests.
/// </summary>
public interface ITimeProvider
{
    DateTimeOffset GetUtcNow();
}