---
layout: page
title: "Search Service"
permalink: /search-service/
---

The `SearchService` is a relatively low-level primitive class that allows you to execute
arbitrary searches with arbitrary parameters. To use it simply request an instance of `ISearchService`
from your service provider:
```c#
var searchService = ServiceProvider.GetRequiredService<ISearchService>();
```
Or do it through dependency injection (preferred):
```c#
public class MyService {
    private readonly ISearchService _searchService;
    
    public MyService(ISearchService searchService) {
        _searchService = searchService;
    }
}
```

Once you have the instance you should create a `SearchParameters` object:
```c#
// 'GetAssets' is the name of the search to execute
// 2 is the page to load (Page)
// 20 is how many assets to load per page. (PageCount) 
// Page and PageCount is option, and defaults to 1 and 12 respectively.
var parameters = new SearchParameters("GetAssets", 2, 20)
{
    {"sCatalogFolderId", "40"}
};
```

The `parameters` object should then be passed to the search service to be executed:
```c#
var response = await searchService.Search<GetAssetsResponse>(parameters);
```

The `response` objects has your response items in the `Items`. 

Additionally the `response` object contains quite a few helper methods and properties for navigating
the response and requesting more data:

Property|Description
--------|-----------
`Items`|The items that was actually returned from the request.
`Total`|How many items are available in total if you keep searching through the pages.
`TotalPages`|How many pages are available in total, assuming the current page size.
`Next`|Returns a `SearchParameters` object for getting the next page of results.
`Previous`|Same as `Next` but in the other direction.
`IsLast`|Returns true if this is the last page of responses. Useful for looping over all the data.
`GoToPage(int page)`|Returns a `SearchParameters` object for loading that specific page.

Using the navigation properties on the `response` object automatically ensures that all 
pages are being loaded using the same type of response, so you don't need to keep specifying 
the generic. 


## Tips and tricks
This section contains some general tips and tricks for using the `SearchService`. 

### `SearchParameters` class
The following helper properties can make working with the `SearchParameters` object a bit easier:

Property|Description
--------|-----------
`Page`|Get or set the page being loaded.
`PageSize`|Get or set the size of each individual page.
`Method`|Get or set the name of the method being executed.
`SearchName`|Get or set the name of the search being executed.  

In addition methods are provided for adding or removing general parameters from the object:

Method|Description
------|-----------
Set(string key, IEnumerable<string> values)|Sets the specific string values.
Set(string key, int value)|Sets a specific single string value.
Set(string key, IEnumerable<int> values)|Sets the specific int values.
SetDateBetween(string key, DateTime from = default, DateTime to = default)|Sets the parameters for doing a `datebetween` search. 


Additionally it is possible the specify the response type directly on the `SearchParameters<T>` generic, 
then you don't have to specify it when you actually execute the search.