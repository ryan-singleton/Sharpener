<img src="images/sharpener-logo-40.png"
align="right"
style="height: 40px;" />

# Sharpener.Rest

A small scoped package that makes REST and HTTP requests more fluid and pleasurable to write using extensibility and shallow layers of code.

## Table of Contents

- [Sharpener.Json](#sharpenerjson)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Installation](#installation)
  - [Usage](#usage)
    - [WriteJson](#writejson)
    - [ReadJsonAs`<TResult>`](#readjsonastresult)

## Features

The `Sharpener.Rest` package focuses on features for the basic runtime layer's web request tooling.

- Fluent syntax for generating REST requests
- `HttpClient` response mocking made simpler

## Installation

Navigate to the directory of your .csproj file and then run this command
`dotnet add package Sharpener.Rest`
or
`PM> Install-Package Sharpener.Rest`

## Usage

### Fluent Requests

Here's an example that should demonstrate the features succinctly.

```cs
// you should get your HttpClient from a factory but..
var httpClient = new HttpClient();
var result = await httpClient.Rest("https://nice-service.com/api")
    .SetPaths("accounts", "create")
    .SetBearerToken(token)
    .AddQueries(new
    {
        Expired = false,
        Owner = true,
    })
    .SetJsonContent(createAccount)
    .PostAsync()
    .ReadJsonAs<CreatedAccount>();

var createdAccount = result.IsSuccess
    ? result.Value
    : // handle failures
```

To create an instance of `RestRequest`, you need to provide an `HttpClient` with a base address. The base address is essential for building the request URL.

```csharp
var httpClient = new HttpClient();
var request = httpClient.Rest("https://api.example.com");
```

This method adds a form URL encoded parameter to the request.

### Appending Path Segments

```csharp
request.SetPaths("users", "123", "profile");
```

This method adds path segments to the request URL. The segments are concatenated with slashes ("/").

### Sending HTTP Methods

The `RestRequest` class provides methods for sending different HTTP methods: GET, POST, PUT, PATCH, and DELETE.

```csharp
var response = await request.GetAsync();
var response = await request.PostAsync();
var response = await request.PutAsync();
var response = await request.PatchAsync();
var response = await request.DeleteAsync();
```

### Adding Query Parameters

```csharp
request.AddQuery("page", 1)
       .AddQuery("pageSize", 10);
```

```csharp
request.AddQueries(new
{
    page = 1,
    pageSize = 10
})
```

These methods allow you to set query parameters for the request URL.

### Adding Basic Authentication Token

```csharp
request.SetBasicToken("username", "password");
```

This method adds a Basic authentication token to the request's `Authorization` header.

### Setting Form Content

```csharp
request.SetFormContent(new
{
    Name = "Bilbo Baggins",
    City = "The Shire"
})
```

This method sets the request content as form URL encoded data.

### Adding JSON Content

```csharp
var data = new { name = "John", age = 30 };
request.SetJsonContent(new
{
    Name = "Smeagol",
    City = "Man, He's in a Cave"
});
```

This method adds the provided object as a JSON payload in the request.

### Adding a JWT Bearer Token

```csharp
request.SetBearerToken("access_token");
```

This method adds a bearer token to the request's `Authorization` header.

## Customization and Execution

Once you have built the `RestRequest` instance, you can execute the request using the provided HTTP methods (`GetAsync`, `PostAsync`, etc.). These methods return a `Task<HttpResponseMessage>` representing the HTTP response.

```csharp
var response = await request.GetAsync();
```

You can further customize the request by directly accessing the `Request` property of the `RestRequest` instance, which is an instance of `HttpRequestMessage`.

```csharp
request.Request.Headers.Add("CustomHeader", "Value");
```

## Retrying

If you need resiliency in your requests, there is a default retry that can be used.

```csharp
request.UseRetry();
```

This uses an extension method that is available to any `Func<Task<HttpResponseMessage>>` that is also called `UseRetry()`.
You can customize the retry like this:

```csharp
request.UseRetry(options =>
{
    options.MaxAttempts = 5;
    options.Delay = TimeSpan.FromSeconds(2);
    options.UseBackoff = true;
    options.BackoffFactor = 3;
    options.SetAcknowledgement((attempt, statusCode) =>  Console.WriteLine($"Attempt number {attempt} failed with status code of {statusCode}"));
    // the status code returned cannot be one that we typically retry
    // the content must deserialize correctly with the right value in the name property
    options.SetRequirement(message => !message.IsRetryStatusCode() && (await message.ReadContentJsonAs<Item>())?.Name == "John";);
});
```

Although the defaults that you have with a simple `UseRetry()` will result in the same results as this:

```csharp
request.UseRetry(options =>
{
    options.MaxAttempts = 3;
    options.Delay = TimeSpan.FromSeconds(1);
    options.UseBackoff = true;
    options.BackoffFactor = 2;
    options.SetAcknowledgement(null)
    options.SetRequirement(message => !message.IsRetryStatusCode());
});
```

This will often be sufficient, in which case, keep it short and sweet with `UseRetry()`.
