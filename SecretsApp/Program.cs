using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

// bypass hack til self certificatede vault
var EndPoint = "https://localhost:8201/";
var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback =
(message, cert, chain, sslPolicyErrors) => { return true; };

// forbindelse til vault
// Initialize one of the several auth methods.
IAuthMethodInfo authMethod =
new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
// Initialize settings. You can also set proxies, custom delegates etc. here.
var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
{
    Namespace = "",
    MyHttpClientProviderFunc = handler
    => new HttpClient(httpClientHandler)
    {
        BaseAddress = new Uri(EndPoint)
    }
};
IVaultClient vaultClient = new VaultClient(vaultClientSettings);

// her hentes secrets fra mountet secret
// Use client to read a key-value secret.
Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2
.ReadSecretAsync(path: "hemmeligheder", mountPoint: "secret");
var minkode = kv2Secret.Data.Data["MinKode"];
Console.WriteLine($"MinKode: {minkode}");