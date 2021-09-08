# Digizuite C# SDK
Welcome to the Digizuite C# SDK documentation. At this site you will find documentation on how to use
the sdk and some tips and tricks for interacting with the Digizuite in general. 

Feel free to file an issue if you find any problems, or have any questions about how to use the SDK. 

This project is still very much in development, is only compatible with Digizuite DAM from version 
5.3 and forward.

## Getting started
The Digizuite SDK consists of two main packages

- Digizuite.SDK
- Digizuite.SDK.Core

The Digizuite.SDK contains the actual SDK and the Digizuite.SDK.Core contains the initialization code using dependency injection from `Microsoft.Extensions.DependencyInjection`. Digizuite.SDK.Core depends on Digizuite.SDK.
We recommend using the default initialization we provide, however, it is possible to install only the SDK and build it from scratch using other dependency injectors (e.g. AutoFac).

To get started with the Digizuite SDK, you should first install the Digizuite packages from nuget:

If you are using dotnet core:
```bash
dotnet add package Digizuite.Sdk.Core
```

Or if you are on .Net Framework
```powershell
Install-Package Digizuite.Sdk.Core
```

Once the packages are installed, there are a few things that must be done

- Implement a logger
- Initialize everything

The Digizuite SDK is logging information and in order to utilize this, one must implement a logger. This can be achieved by implementing the interface `Digizuite.Logging.ILogger<T>`. A simple ConsoleLogger example can be found [here](https://github.com/Digizuite/Digizuite.SDK/blob/master/Digizuite.Core/Logging/ConsoleLogger.cs).

Additionally the SDK has to be initialized. If you are using dotnet, an extension method `AddDigizuite` is provided. If you are using .Net Framework the initialization must be done by hand. Use the extension method `AddDigizuite` for inspiration.

A simple example of how the initialization code looks in a console application is as follows

```c#
var serviceCollection = new ServiceCollection(); 

var config = new DigizuiteConfiguration()
{
	BaseUrl = new Uri("https://<Digizuite url>.com/"),
	SystemUsername = "<Username>",
	SystemPassword = "<Password>"
};

serviceCollection.AddDigizuite(config);
serviceCollection.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));
var serviceProvider = serviceCollection.BuildServiceProvider(true);
```

To test that everything is working, use the registered serviceCollection to get an implementation of one of the Digizuite services (e.g. `ISearchService`) and execute a default search for getting assets

```c#

var searchService = serviceProvider.GetRequiredService<ISearchService>();

var parameters = new SearchParameters("GetAssets")
{
	{"sCatalogFolderId", "40"}
};

var results = await searchService.Search<Digizuite.Models.Asset>(parameters);
```

For additional full examples, check out the 
[samples project on GitHub](https://github.com/Digizuite/Digizuite.SDK/tree/master/Digizuite.Samples). 

## Specific documentation

The Digizuite SDK contains multiple services each responsible for an area of Digizuite.

Specific documentation for each individual service can be found at the following pages:

Service|Description
-------|-----------
[Search Service](search-service.md)|A low level utility for executing general searches against the Digizuite.
