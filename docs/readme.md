# Digizuite C# SDK
Welcome to the Digizuite C# SDK documentation. At this site you will find documentation on how to use
the sdk and some tips and tricks for interacting with the Digizuite in general. 

Feel free to file an issue if you find any problems, or have any questions about how to use the SDK. 

## Getting started
To get started with the Digizuite SDK, you should first install the Digizuite package from nuget:

If you are using dotnet core:
```bash
dotnet install Digizuite Digizuite.Core
```

Or if you are on .Net Framework
```powershell
Install-Package Digizuite Digizuite.Framework
```


Once the packages are installed you can start using them. We recommend using a dependency injection
system like `Microsoft.Extensions.DependencyInjection` to manage the Digizuite services. In fact
if you are using dotnet core, we provide an extension method `AddDigizuite` for quickly adding 
the Digizuite services to your service provider. You should also make sure to register an instance
of `Digizuite.Models.Configuration` so the services knows how to interact with the Digizuite. 

To test that everything is working you can fire of a basic search using `ISearchService`:
```c#

var searchService = ServiceProvider.GetRequiredService<ISearchService>();

var parameters = new SearchParameters("GetAssets")
{
    {"sCatalogFolderId", "40"}
};

var results = await searchService.Search<GetAssetsResponse>(parameters);
```

