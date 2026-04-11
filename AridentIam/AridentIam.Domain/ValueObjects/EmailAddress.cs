using System.Text.RegularExpressions;
using AridentIam.Domain.Common;

namespace AridentIam.Domain.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    private static readonly Regex Pattern =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    public EmailAddress(string value)
    {
        var normalized = Guard.AgainstNullOrWhiteSpace(value, nameof(value)).Trim().ToLowerInvariant();

        if (normalized.Length > 320)
            throw new DomainException("Email must not exceed 320 characters.");

        if (!Pattern.IsMatch(normalized))
            throw new DomainException("Email format is invalid.");

        Value = normalized;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}