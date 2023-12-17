using Amazon.Route53;
using Amazon.Route53.Model;
using Amazon.Route53Domains;
using Amazon.S3;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsRoute53Service : IAwsRoute53Service
{
    private AmazonRoute53Client _client;

    public AwsRoute53Service(AmazonRoute53Client client)
    {
        _client = client;
    }

    public async Task<ValidationResponse<Shared.Models.HostedZone>> GetHostedZonesAsync()
    {
        var response = await _client.ListHostedZonesAsync();
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            return ValidationResponse<Shared.Models.HostedZone>.Failure(message: "", code: response.HttpStatusCode);
        }

        var hostedZones = response.HostedZones.Select(zone => new Shared.Models.HostedZone
        {
            Id = zone.Id,
            Name = zone.Name
        });

        return ValidationResponse<Shared.Models.HostedZone>.Success(hostedZones);
    }
}
