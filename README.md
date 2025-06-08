# Json Placeholder Wrapper Service 
In this branch, we added the `[Authorize(AuthenticationSchemes = "BasicAuthentication")]` attribute to our `Posts` and `Users` controller class.

## Configured SwaggerGen for Authentication
```C#
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JSON Placeholder Wrapper Service", Version = "v1" });

    // Add Security Scheme
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter your username and password for authentication.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new string[] {} // No predefined scopes, allows all authenticated users
        }
    });
});
```