using Microsoft.Data.SqlClient;

namespace AridentIam.Infrastructure.Configuration;

public static class DatabaseConnectionStringFactory
{
    public static string Build(DatabaseSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        if (string.IsNullOrWhiteSpace(settings.Host))
        {
            throw new InvalidOperationException("Database Host is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.Database))
        {
            throw new InvalidOperationException("Database name is not configured.");
        }

        var dataSource = settings.Port.HasValue && settings.Port.Value > 0
            ? $"{settings.Host},{settings.Port.Value}"
            : settings.Host;

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = dataSource,
            InitialCatalog = settings.Database,
            IntegratedSecurity = settings.TrustedConnection,
            Encrypt = settings.Encrypt,
            TrustServerCertificate = settings.TrustServerCertificate,
            MultipleActiveResultSets = settings.MultipleActiveResultSets,
            ConnectTimeout = settings.CommandTimeoutSeconds
        };

        if (!settings.TrustedConnection)
        {
            if (string.IsNullOrWhiteSpace(settings.Username))
            {
                throw new InvalidOperationException(
                    "Database Username must be configured when TrustedConnection is false.");
            }

            if (string.IsNullOrWhiteSpace(settings.Password))
            {
                throw new InvalidOperationException(
                    "Database Password must be configured when TrustedConnection is false.");
            }

            builder.UserID = settings.Username;
            builder.Password = settings.Password;
        }

        return builder.ConnectionString;
    }
}