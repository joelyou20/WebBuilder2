using Amazon.Runtime;
using Azure;
using System.Net;

namespace WebBuilder2.Server.Utils;

public static class AmazonServiceResponseValidator<TException> where TException : AmazonServiceException
{
    
    /// <summary>
    /// Validates the Amazon Web Service response and throws an exception if the response is null or the HTTP status code is not OK.
    /// </summary>
    /// <param name="response">The Amazon Web Service response to validate.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="amazonServiceExceptionType"/> is not derived from <see cref="AmazonServiceException"/>.
    /// </exception>
    /// <exception cref="AmazonServiceException">
    /// Thrown if the <paramref name="response"/> is null or if the HTTP status code is not OK.
    /// </exception>
    public static void Validate<T>(T response, string? message = null) where T : AmazonWebServiceResponse
    {
        if (response == null && Activator.CreateInstance(typeof(TException), message) is AmazonServiceException nullException)
        {
            throw nullException;
        }

        if (response != null &&
            response.HttpStatusCode != HttpStatusCode.OK &&
            Activator.CreateInstance(typeof(TException), $"Response Code: {response.HttpStatusCode}") is AmazonServiceException not200OKException)
        {
            throw not200OKException;
        }
    }
}