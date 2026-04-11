namespace AridentIam.Infrastructure.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "Database";

    public string Host { get; init; } = string.Empty;

    public int? Port { get; init; }

    public string Database { get; init; } = string.Empty;

    public string? Username { get; init; }

    public string? Password { get; init; }

    public bool TrustedConnection { get; init; } = true;

    public bool Encrypt { get; init; } = true;

    public bool TrustServerCertificate { get; init; } = false;

    public bool MultipleActiveResultSets { get; init; } = false;

    public int CommandTimeoutSeconds { get; init; } = 30;
}