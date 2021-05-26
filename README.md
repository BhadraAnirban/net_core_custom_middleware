# Create Custom Middleware in .net core

## Create a normal class-
### Constructor will take the RequestDelegate. This will be used to call the next middleware
### Create a async Task InvokeAsync which will be invoked while executing the middleware in the pipeline.

```
public class TrackUserMiddleware
{
    public readonly RequestDelegate _next;
    public TrackUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        // Wirte your business logic

        await _next(context);
    }
}

```

## Add the middleware in the Service method-
### Use UseMiddleware to add the middleware in the pipeline

```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use other middlewares

    app.UseMiddleware<TrackUserMiddleware>();

    // Use other middlewares
}

```

That's it. Middleware will work.

### In the uploaded files we have we have written the code where there is a singleton object (TrackUser), which have a dictionary to Track the logged in user without calling Database. This will be helpful to verify refresh token and by logged out from any other sppliaction(single sign on), we can revome the user from the dictionary and implement the auto logged out mechanism.

