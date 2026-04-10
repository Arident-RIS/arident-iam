using System.Text.RegularExpressions;
using AridentIam.Domain.Common;

namespace AridentIam.Domain.ValueObjects;

public sealed class Username : ValueObject
{
    private static readonly Regex Pattern = new("^[a-zA-Z0-9._-]{3,50}$", RegexOptions.Compiled);

    public string Value { get; }

    public Username(string value)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(value, nameof(value));
        if (!Pattern.IsMatch(normalized)) throw new DomainException("Username format is invalid.");
        Value = normalized;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
