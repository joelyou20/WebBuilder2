﻿using Amazon.S3.Model;
using WebBuilder2.Shared.Models;

namespace WebBuilder2.Server.Services.Contracts
{
    public interface IAwsS3Service
    {
        Task<IEnumerable<Site>> GetSitesAsync();
    }
}
