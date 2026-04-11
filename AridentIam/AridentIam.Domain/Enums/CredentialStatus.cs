namespace AridentIam.Domain.Enums;

public enum CredentialStatus
{
    Active = 1,
    Locked = 2,
    Revoked = 3,
    Expired = 4,
    Compromised = 5
}