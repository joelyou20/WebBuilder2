﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebBuilder2.Server.Data;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230723153317_AddGithubRepositoriesTable")]
    partial class AddGithubRepositoriesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebBuilder2.Server.Data.Models.SiteDTO", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("WebBuilder2.Shared.Models.GithubRepository", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("AllowAutoMerge")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "allowAutoMerge");

                    b.Property<bool>("AllowMergeCommit")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "allowMergeCommit");

                    b.Property<bool>("AllowRebaseMerge")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "allowRebaseMerge");

                    b.Property<bool>("AllowSquashMerge")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "allowSquashMerge");

                    b.Property<bool>("AutoInit")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "autoInit");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DeleteBranchOnMerge")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "deleteBranchOnMerge");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<string>("GitIgnoreTemplate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "gitIgnoreTemplate");

                    b.Property<string>("GitUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "gitUrl");

                    b.Property<bool>("HasDownloads")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "hasDownloads");

                    b.Property<bool>("HasIssues")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "hasIssues");

                    b.Property<bool>("HasProjects")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "hasProjects");

                    b.Property<bool>("HasWiki")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "hasWiki");

                    b.Property<string>("Homepage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "homepage");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "isPrivate");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "isTemplate");

                    b.Property<string>("LicenseTemplate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "licenseTemplate");

                    b.Property<DateTime>("ModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("RepoName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "repoName");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "teamId");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "url");

                    b.Property<bool>("UseSquashPrTitleAsDefault")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "useSquashPrTitleAsDefault");

                    b.Property<int>("Visibility")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "visibility");

                    b.HasKey("Id");

                    b.ToTable("GithubRepositories");
                });
#pragma warning restore 612, 618
        }
    }
}
