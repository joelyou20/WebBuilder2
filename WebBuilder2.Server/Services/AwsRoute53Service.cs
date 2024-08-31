using Amazon.CertificateManager;
using Amazon.Route53;
using Amazon.Route53.Model;
using Amazon.Route53Domains;
using System.Net;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Utils;
using WebBuilder2.Server.Utils.Extensions;

namespace WebBuilder2.Server.Services;

public class AwsRoute53Service(AmazonRoute53Client client) : IAwsRoute53Service
{
    private readonly AmazonRoute53Client _client = client;

    public async Task<IEnumerable<Shared.Models.HostedZone>> GetHostedZonesAsync()
    {
        ListHostedZonesResponse response = await _client.ListHostedZonesAsync();

        AmazonServiceResponseValidator<AmazonRoute53Exception>.Validate(response, "Failed to get hosted zones.");

        var hostedZones = response.HostedZones.Select(zone => new Shared.Models.HostedZone
        {
            Id = zone.Id,
            Name = zone.Name
        });

        return hostedZones;
    }
}
