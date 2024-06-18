namespace WebBuilder2.Client.Models;

public enum JobType
{
    CreateSite,
    RegisterDomain,
    CreateBuckets,
    ConfigureAWSLogging,
    CreateRepository,
    AddRepositorySecrets,
    AddScriptsToRepository,
    ScaffoldRepository,
    AllowPublicAccess,
    AddBucketPolicy
}
