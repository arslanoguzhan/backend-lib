namespace BackendLib;

/// <summary>
/// C# version of Maybe monad for classes, Nullable[T] was for structs.
/// </summary>
/// <typeparam name="T">The type inside the optional</typeparam>
public readonly struct Optional<T>
    where T : class
{
    private readonly T? _value;

    public Optional(T? value)
    {
        _value = value;
    }

    public T Value => _value ?? throw new Exception("missing value");

    public bool HasValue => _value is not null;

    public readonly T? GetValueOrDefault() => _value;

    public readonly T GetValueOrDefault(T defaultValue) => _value ?? defaultValue;

    public override string? ToString() => _value?.ToString() ?? "";

    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    public override bool Equals(object? obj)
        => obj is Optional<T> other
            && other.HasValue == HasValue
            && (!HasValue || Value.Equals(other.Value));

    public static bool operator ==(Optional<T> left, Optional<T> right) => left.Equals(right);

    public static bool operator !=(Optional<T> left, Optional<T> right) => !(left == right);

    public static implicit operator Optional<T>(T? value) => new(value);
}