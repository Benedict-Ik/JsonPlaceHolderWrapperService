# Json Placeholder Wrapper Service 
In this branch, we configured a typed HTTP Client in Program.cs, created and implemented a JsonPlaceholderService 

## Configured Typed HTTP Client
Added the below line to appsettings:
```JSON
"BaseUrl": "https://jsonplaceholder.typicode.com/"
```

Added the below line to Program.cs:
```CSharp
var baseUrl = configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri(baseUrl);
});
```


The above code registers an HTTP client named `"JsonPlaceholder"` in the application's dependency injection container using 
`AddHttpClient`. It configures the client by setting its base address to https://jsonplaceholder.typicode.com/, meaning all 
requests using this client will be relative to this URL. Additionally, it adds a default request header to specify that the 
client expects responses in JSON format. This setup allows the application to efficiently manage HTTP requests while keeping 
the configuration centralized and reusable.

## Created and implemented an interface
- Created an Interfaces and Services folders that houses the `IJsonPlaceholderService` and `JsonPlaceholderService` respectively.  
- This interface class defines a contract for a service that interacts with the JSONPlaceholder API.  
- The `JsonPlaceholderService` class is the concrete definition of definitions defined in the interface class.

## Registered the newly created service in the DI Container
In Program.cs we inserted the below code to register the service:
```C#
builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();
```