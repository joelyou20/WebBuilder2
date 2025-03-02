using Amazon.CostExplorer;
using Amazon.Route53;
using Amazon.Route53Domains;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.CertificateManager;
using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Octokit;
using WebBuilder2.Server.Services.Contracts;
using WebBuilder2.Server.Options;
using Microsoft.AspNetCore.Identity;
using WebBuilder2.Shared.Models;
using WebBuilder2.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebBuilder2.Server.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHubClient(this IServiceCollection services, Func<IServiceProvider, IAwsSecretsManagerService> serviceProvider, ConfigurationManager configuration)
        {
            var awsSecretsManagerService = serviceProvider.Invoke(services.BuildServiceProvider());
            
            var pat = awsSecretsManagerService.GetSecretAsync(AwsSecret.GithubPat).Result;

            var githubSettings = configuration.GetSection(nameof(GithubOptions)).Get<GithubOptions>()!;
            services.AddSingleton<IGitHubClient, GitHubClient>(sp =>
            {
                var client = new GitHubClient(new ProductHeaderValue(githubSettings.OrganizationName))
                {
                    Credentials = new Credentials(pat)
                };
                return client;
            });

            return services;
        }

        public static IServiceCollection AddAwsS3Client(this IServiceCollection services)
        {
            AmazonS3Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonS3Client(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsRoute53Client(this IServiceCollection services)
        {
            AmazonRoute53Config awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonRoute53Client(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsRoute53DomainsClient(this IServiceCollection services)
        {
            AmazonRoute53DomainsConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddSingleton(sp => new AmazonRoute53DomainsClient(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsCostExplorerClient(this IServiceCollection services)
        {
            AmazonCostExplorerConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonCostExplorerClient(credentials, awsConfig));
        }

        public static IServiceCollection AddAwsSecretsManagerClient(this IServiceCollection services)
        {
            AmazonSecretsManagerConfig awsConfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonSecretsManagerClient(credentials, awsConfig));
        }

        public static IServiceCollection AddAdSenseService(this IServiceCollection services, Func<IServiceProvider, IAwsSecretsManagerService> serviceProvider, ConfigurationManager configuration)
        {
            var awsSecretsManagerService = serviceProvider.Invoke(services.BuildServiceProvider());

            var clientSecret = awsSecretsManagerService.GetSecretAsync(AwsSecret.GoogleClientSecret).Result;

            var googleSettings = configuration.GetSection(nameof(GoogleOptions)).Get<GoogleOptions>()!;

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = googleSettings.ClientId,
                        ClientSecret = clientSecret,
                    },
                    new string[] { AdsenseService.Scope.Adsense },
                    "WebBuilder2",
                    CancellationToken.None).Result;

            return services.AddScoped(sp => new AdsenseService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            }));
        }

        public static IServiceCollection AddAwsCertificateManagerClient(this IServiceCollection services)
        {
            AmazonCertificateManagerConfig awsconfig = new()
            {
                UseAlternateUserAgentHeader = AwsConfig.UseAlternateUserAgentHeader,
                RegionEndpoint = AwsConfig.RegionEndpoint
            };

            var credentials = AwsAuthenticationHelper.LoadDefaultProfile();

            return services.AddScoped(sp => new AmazonCertificateManagerClient(credentials, awsconfig));
        }

        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/identity/account/login";
                options.AccessDeniedPath = "/identity/account/accessDenied";
                options.Cookie.Name = "WebBuilder2Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
