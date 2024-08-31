using FizzWare.NBuilder;
using Google.Apis.Adsense.v2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Moq;
using static Google.Apis.Adsense.v2.AccountsResource;

namespace Webbuilder2.Server.Tests.Fakes;

public class FakeAdsenseService : AdsenseService
{
    public FakeAdsenseService() : base() { }

    public override FakeAccountsResource Accounts { get; } = new();
    public override string BasePath => "test_basePath";
}

public class FakeAccountsResource : AccountsResource
{
    public FakeAccountsResource() : base(new Mock<IClientService>().Object) { }

    public new static FakePaymentsResource PaymentsResource { get; } = new();
    public override FakePaymentsResource Payments { get; } = new();

    public override FakeGetRequest Get(string name) => Builder<FakeGetRequest>.CreateNew()
        .With(x => x.Name, name)
        .Build();
}

public class FakePaymentsResource : PaymentsResource
{
    public FakePaymentsResource() : base(new Mock<IClientService>().Object) { }

    public new static FakeListRequest ListRequest { get; } = new();

    public override FakeListRequest List(string parent) => Builder<FakeListRequest>.CreateNew()
        .With(x => x.Parent, parent)
        .Build();
}

public class FakeListRequest : PaymentsResource.ListRequest
{
    public FakeListRequest() : base(new Mock<IClientService>().Object, "test_parent") { }
}

public class FakeGetRequest : GetRequest
{
    public FakeGetRequest() : base(new Mock<IClientService>().Object, "test_account") { }

}

public class FakeClientServiceRequest<TResponse> : ClientServiceRequest<TResponse> where TResponse : class
{
    public FakeClientServiceRequest() : base(new Mock<IClientService>().Object) { }

    public override string MethodName => "test_methodName";

    public override string RestPath => "test_restPath";

    public override string HttpMethod => "test_httpMethod";

    public new async Task<TResponse> ExecuteAsync()
    {
        var obj = new
        {
            Key = "test_value"
        } as TResponse;

        return await Task.FromResult(obj!);
    }
}
