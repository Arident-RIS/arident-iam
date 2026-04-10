namespace AridentIam.Domain.Enums;

public enum SyncJobType
{
    FullImport = 1,
    IncrementalImport = 2,
    AttributeRefresh = 3,
    RelationshipRefresh = 4
}