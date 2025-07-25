//[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]
using Microsoft.Xrm.Tooling.Connector;

public class MSTestSettings
{
    private static string Url = "";
    private static string ClientId = "";
    private static string ClientSecret = "";

    private static string connectionString = $@"AuthType=ClientSecret;url={Url};ClientId={ClientId};ClientSecret={ClientSecret}";

    public CrmServiceClient service;

    public MSTestSettings()
    {
        service = GetService();
    }

    public static CrmServiceClient GetService()
    {
        CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
        return crmServiceClient;
    }
}
