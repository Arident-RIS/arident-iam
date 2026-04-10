namespace AridentIam.Domain.Common;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException($"{paramName} is required.");
        return value.Trim();
    }

    public static string AgainstMaxLength(string? value, int maxLength, string paramName)
    {
        var trimmed = AgainstNullOrWhiteSpace(value, paramName);
        if (trimmed.Length > maxLength)
            throw new DomainException($"{paramName} must not exceed {maxLength} characters.");
        return trimmed;
    }

    public static Guid AgainstDefault(Guid value, string paramName)
    {
        if (value == Guid.Empty) throw new DomainException($"{paramName} is required.");
        return value;
    }

    public static void AgainstInvalidRange(DateTimeOffset? from, DateTimeOffset? to, string paramName)
    {
        if (from.HasValue && to.HasValue && from > to)
            throw new DomainException($"{paramName} has an invalid date range.");
    }

    public static void AgainstNull<T>(T? input, string paramName) where T : class
    {
        if (input is null) throw new ArgumentNullException(paramName);
    }

    public static int AgainstNegative(int value, string paramName)
    {
        if (value < 0) throw new DomainException($"{paramName} cannot be negative.");
        return value;
    }

    public static T AgainstOutOfRange<T>(T value, T min, T max, string paramName)
        where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new DomainException($"{paramName} must be between {min} and {max}.");
        return value;
    }

    public static T AgainstInvalidEnum<T>(T value, string paramName)
        where T : struct, Enum
    {
        if (!Enum.IsDefined(value))
            throw new DomainException($"'{value}' is not a valid value for {paramName}.");
        return value;
    }
}
