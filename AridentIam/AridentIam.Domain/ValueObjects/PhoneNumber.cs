using System.Text.RegularExpressions;
using AridentIam.Domain.Common;

namespace AridentIam.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    private static readonly Regex Pattern = new(@"^[0-9+()\-\s]{7,20}$", RegexOptions.Compiled);

    public string Value { get; }

    public PhoneNumber(string value)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(value, nameof(value));
        if (!Pattern.IsMatch(normalized)) throw new DomainException("Phone number format is invalid.");
        Value = normalized;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
