# Json Placeholder Wrapper Service 
In this branch, we configured a typed HTTP Client in Program.cs, created and implemented a JsonPlaceholderService 

## Configuring Typed HTTP Client
In Program.cs, we added the below line:
```C#
builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});
```

The above code registers an HTTP client named `"JsonPlaceholder"` in the application's dependency injection container using 
`AddHttpClient`. It configures the client by setting its base address to https://jsonplaceholder.typicode.com/, meaning all 
requests using this client will be relative to this URL. Additionally, it adds a default request header to specify that the 
client expects responses in JSON format. This setup allows the application to efficiently manage HTTP requests while keeping 
the configuration centralized and reusable.

## Creating and implementing an interface
- Created an Interfaces and Services folders that houses the `IJsonPlaceholderService` and `JsonPlaceholderService` respectively.  
- This interface class defines a contract for a service that interacts with the JSONPlaceholder API.  
- The `JsonPlaceholderService` class is the concrete definition of definitions defined in the interface class.

## Registering the newly created service in the DI Container
In Program.cs we inserted the below code to register the service:
```C#
builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();
```