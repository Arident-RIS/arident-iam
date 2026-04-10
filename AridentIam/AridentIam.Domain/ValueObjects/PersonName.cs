using AridentIam.Domain.Common;

namespace AridentIam.Domain.ValueObjects;

public sealed class PersonName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    public PersonName(string firstName, string lastName)
    {
        FirstName = Guard.AgainstNullOrWhiteSpace(firstName, nameof(firstName));
        LastName = Guard.AgainstNullOrWhiteSpace(lastName, nameof(lastName));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
