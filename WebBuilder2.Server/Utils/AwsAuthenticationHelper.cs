using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using System.Diagnostics;
using Amazon.S3;

namespace WebBuilder2.Server.Utils
{
    public static class AwsAuthenticationHelper
    {
        // Method to get SSO credentials from the information in the shared config file.
        // Required to have a file at the path "C:\Users\USERNAME\.aws\credentials"
        // NOTE: File type of credentials must have no extension
        public static AWSCredentials? LoadDefaultProfile()
        {
            SharedCredentialsFile credentialsFile = new();
            var allProfiles = credentialsFile.ListProfiles();
            var defaultProfile = allProfiles.Where(profile => profile.Name.Equals("default")).Single();
            AWSCredentials? awsCredentials = defaultProfile.GetAWSCredentials(credentialsFile);

            return awsCredentials;
        }
    }
}
