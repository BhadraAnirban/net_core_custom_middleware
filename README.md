# net_core_custom_middleware
Create Custom Middleware in .net core

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

That's it.

### In the uploaded files we have we have written the code where there is a singleton object, whcih have a dictionary to Track the logged in user without calling Database.

