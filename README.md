# Json Placeholder Wrapper Service 
## Overview
Here, we introduced global exception handling and authentication failure handling in the application. The `ExceptionMiddleware` class ensures that any unhandled exceptions are caught, logged, and returned as structured JSON responses, preventing the application from crashing unexpectedly. The `AuthenticationFailureHandler` class customizes authentication failures by sending appropriate error responses when authentication or authorization fails, ensuring clients receive meaningful feedback. Additionally, `ErrorResponse` model defines the standard structure for error responses. By configuring authentication with options.ForwardChallenge and options.ForwardForbid, failed authentication attempts are forwarded to the handler, ensuring proper error management across the application.

## Implemented `Try..Catch` Error Hnadling in Service and Controller classes
Added try...catch error handling mechanism to `Posts` and `Users` controllers, and `JsonPlaceholderService` class.

## Created `Error Response` class in Models folder
Created the below class in the `Models` folder:
```C#
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = "";
    public string? Details { get; set; } 
}
```

## Implemented Global Exception Handling Middleware
Created a custom middleware called `ExceptionMiddleware` in the `Helpers` folder

## Registered the Custom Middleware in Program.cs
Added the below line just before `app.UseAuthentication()`:
```C#
app.UseMiddleware<ExceptionMiddleware>();
```

## Created a custom AuthenticationFailureHandler class
Created a custom `AuthenticationFailureHandler` class to handle Authentication Failures in the `Helpers` folder.

## Updated Program.cs file with the below lines of code
```C#
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options =>
    {
        options.ForwardChallenge = "BasicAuthenticationFailure";
        options.ForwardForbid = "BasicAuthenticationFailure";
    });
```