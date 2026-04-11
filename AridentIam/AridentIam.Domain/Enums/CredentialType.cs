namespace AridentIam.Domain.Enums;

public enum CredentialType
{
    Password = 1,
    TotpSecret = 2,
    RecoveryCode = 3,
    WebAuthn = 4,
    ApiSecret = 5,
    Certificate = 6
}