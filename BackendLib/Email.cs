using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BackendLib;

/// <summary>
/// Non-empty and valid email-address.
/// </summary>
[DebuggerDisplay("{Value}")]
public class Email : IComparable<Email>, IEquatable<Email>
{
    private static readonly EmailAddressAttribute _validator = new();

    public string Value { get; }

    public string UserName { get; }

    public string DomainName { get; }

    private Email(string value)
    {
        var parts = value.Split('@');
        Value = value;
        UserName = parts[0];
        DomainName = parts[1];
    }

    public static Email FromString(string? emailValue)
    {
        emailValue = emailValue?.Trim() ?? "";
        if (!_validator.IsValid(emailValue))
        {
            throw new Exception("email format is invalid");
        }

        return new Email(emailValue);
    }

    public IEnumerator<char> GetEnumerator() => Value.GetEnumerator();

    public int CompareTo(Email? other) => string.Compare(Value, other?.Value ?? "", StringComparison.Ordinal);

    public bool Equals(Email? other) => Value.Equals(other?.Value ?? "");

    public override bool Equals(object? obj) => Value.Equals(obj);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(Email? left, Email? right) => (left?.Value ?? "") == (right?.Value ?? "");

    public static bool operator !=(Email? left, Email? right) => !(left == right);

    public static bool operator <(Email? left, Email? right) => string.CompareOrdinal(left?.Value ?? "", right?.Value ?? "") < 0;

    public static bool operator <=(Email? left, Email? right) => string.CompareOrdinal(left?.Value ?? "", right?.Value ?? "") <= 0;

    public static bool operator >(Email? left, Email? right) => string.CompareOrdinal(left?.Value ?? "", right?.Value ?? "") > 0;

    public static bool operator >=(Email? left, Email? right) => string.CompareOrdinal(left?.Value ?? "", right?.Value ?? "") >= 0;
}