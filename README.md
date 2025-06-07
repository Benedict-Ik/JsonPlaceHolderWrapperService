# Json Placeholder Wrapper Service 
In this branch, we added the Basic Authentication middleware, `.AddAuthentication`, created a custom `BasicAuthenticationHelper` class, added the auth credentials in our appsettings file using `Secret Manager` , and enable Authentication/Authorization in the app.

## Configured Authentication using Basic Authentication
Added the below line in Program.cs
```C#
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHelper>("BasicAuthentication", null);
```

The above code configures authentication in an ASP.NET Core application using Basic Authentication. It first registers the authentication service with the scheme name `"BasicAuthentication"`, then adds a custom authentication handler (BasicAuthenticationHandler) that 
inherits from `AuthenticationHandler<AuthenticationSchemeOptions>` and manages user authentication. The helper class is responsible for processing authentication requests, such as validating credentials from an HTTP request header. This setup enables secure access control by requiring clients to provide a username and password when 
interacting with protected endpoints.

## Created `BasicAuthenticationHelper` class
We created a `Helpers` folder that housed our BasicAuthenticationHandler class.

## Created `BasicAuth` object in app.settings
Added the below line in appsettings:
```JSON
"BasicAuth": {
    "Username": "Username",
    "Password":  "Password"
}
```

>The above values are placeholders. We will be using the secret manager package to store the real values.

## Using Secret Manager to store Secrets
For privacy and security, we installed and employed the use of the Secret Manager package, which helps us 
store secrets (sensitive piece of information) securely and only reference them when we need to.

### Important Commands to Note
1. **Initialize Secret Manager**
```Sh
dotnet user-secrets init
```

1. **Storing Secrets**
```Sh
dotnet user-secrets set "BasicAuth:Username" "Username"
dotnet user-secrets set "BasicAuth:Password" "Password"
```

1. **Retrieve all secrets**
```Sh
dotnet user-secrets list
```

In your **Program.cs** file, add the below line to load secrets into configuration:
```C#
builder.Configuration.AddUserSecrets<Program>();
```

## Enabled Authentication and Authorization Middleware
We added the below lines of code in Program.cs:
```C#
app.UseAuthentication();
app.UseAuthorization();
```

The above enables authentication and authorization middleware in the application. `app.UseAuthentication()` ensures that incoming requests are checked for authentication credentials, like tokens or login details. `app.UseAuthorization()` then verifies whether the 
authenticated user has the necessary permissions to access specific resources. Together, they help enforce security by ensuring only authorized users can interact with protected endpoints.