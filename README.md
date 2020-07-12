
# The Sample code for Microsoft 365 Enterprise SharePoint File

## Initialize
Initialize the .NET development secret store by opening your CLI in the directory that contains GraphFileSample.csproj and running the following command.
```
dotnet user-secrets init
```
### AcquireTokenForClient:
Add your application ID and a list of required scopes to the secret store using the following commands. Replace YOUR_APP_ID_HERE, YOUR_APP_TENANT_ID,YOUR_APP_CLIENT_SECRET with the application ID you created in the Azure portal.
```
dotnet user-secrets set appId "YOUR_APP_ID_HERE"
dotnet user-secrets set scopes "https://graph.microsoft.com/.default"
dotnet user-secrets set tenantId "YOUR_APP_TENANT_ID"
dotnet user-secrets set clientSecret "YOUR_APP_CLIENT_SECRET"
dotnet user-secrets set siteId "{YOUR_TENANT}.sharepoint.com:/sites/{YOUR_SITE}"
```
