using Amazon.Route53;
using Amazon.Route53.Model;
using Amazon.Route53Domains;
using Amazon.S3;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Shared.Validation;

namespace WebBuilder2.Server.Services;

public class AwsRoute53Service(AmazonRoute53Client client) : IAwsRoute53Service
{
    private readonly AmazonRoute53Client _client = client;

    public async Task<IEnumerable<Shared.Models.HostedZone>> GetHostedZonesAsync()
    {
        var response = await _client.ListHostedZonesAsync();
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to retrieve list of hosted zones");
        }

        var hostedZones = response.HostedZones.Select(zone => new Shared.Models.HostedZone
        {
            Id = zone.Id,
            Name = zone.Name
        });

        return hostedZones;
    }
}
