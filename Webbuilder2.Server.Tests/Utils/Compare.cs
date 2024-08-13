using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBuilder2.Shared.Models.Dtos;
using WebBuilder2.Shared.Models;

namespace Webbuilder2.Server.Tests.Utils;

public static class Compare
{
    public static void AreEqual(LicenseMetadata expected, GithubProjectLicense actual)
    {
        Assert.Multiple(() =>
        {
            Assert.That(expected.Featured, Is.EqualTo(actual.Featured));
            Assert.That(expected.Name, Is.EqualTo(actual.Name));
            Assert.That(expected.Url, Is.EqualTo(actual.Url));
            Assert.That(expected.Key, Is.EqualTo(actual.Key));
        });
    }

    public static void AreEqual(Repository expected, RepositoryModel actual)
    {
        Assert.Multiple(() =>
        {
            Assert.That(expected.Visibility, Is.Not.Null);
            Assert.That(expected.Visibility.ToString(), Is.EqualTo(actual.Visibility.ToString()));

            Assert.That(expected.AllowAutoMerge, Is.Not.Null);
            Assert.That(expected.AllowAutoMerge, Is.EqualTo(actual.AllowAutoMerge));

            Assert.That(expected.AllowMergeCommit, Is.Not.Null);
            Assert.That(expected.AllowMergeCommit, Is.EqualTo(actual.AllowMergeCommit));

            Assert.That(expected.AllowRebaseMerge, Is.Not.Null);
            Assert.That(expected.AllowRebaseMerge, Is.EqualTo(actual.AllowRebaseMerge));

            Assert.That(expected.AllowSquashMerge, Is.Not.Null);
            Assert.That(expected.AllowSquashMerge, Is.EqualTo(actual.AllowSquashMerge));

            Assert.That(expected.DeleteBranchOnMerge, Is.Not.Null);
            Assert.That(expected.DeleteBranchOnMerge, Is.EqualTo(actual.DeleteBranchOnMerge));

            Assert.That(expected.IsTemplate, Is.EqualTo(actual.IsTemplate));
            Assert.That(expected.Description, Is.EqualTo(actual.Description));
            Assert.That(expected.Private, Is.EqualTo(actual.IsPrivate));
            Assert.That(expected.HasWiki, Is.EqualTo(actual.HasWiki));
            Assert.That(expected.GitUrl, Is.EqualTo(actual.GitUrl));
            Assert.That(expected.HasDownloads, Is.EqualTo(actual.HasDownloads));
            Assert.That(expected.HasIssues, Is.EqualTo(actual.HasIssues));
            Assert.That(expected.HasWiki, Is.EqualTo(actual.HasWiki));
            Assert.That(expected.Homepage, Is.EqualTo(actual.Homepage));
            Assert.That(expected.Id, Is.EqualTo(actual.ExternalId));
            Assert.That(expected.Private, Is.EqualTo(actual.IsPrivate));
            Assert.That(expected.IsTemplate, Is.EqualTo(actual.IsTemplate));
            Assert.That(expected.CreatedAt.DateTime, Is.EqualTo(actual.ModifiedDateTime));
            Assert.That(expected.Name, Is.EqualTo(actual.Name));
            Assert.That(expected.FullName, Is.EqualTo(actual.RepoName));
            Assert.That(expected.HtmlUrl, Is.EqualTo(actual.HtmlUrl));
            Assert.That(expected.CreatedAt.DateTime, Is.EqualTo(actual.CreatedDateTime));
        });
    }
}
