using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddMvc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSession();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("<html><body>");
    await context.Response.WriteAsync("<h1>WebApp6BySudin</h1>");
    await context.Response.WriteAsync("<h2><u>State Management Demo Q.25</u></h2>");
    await context.Response.WriteAsync("<ul>");
    await context.Response.WriteAsync("<li><a href='/session'>Session State</a></li><br>");
    await context.Response.WriteAsync("<li><a href='/httpcontext'>HttpContext</a></li><br>");
    await context.Response.WriteAsync("<li><a href='/tempdata'>TempData</a></li><br>");
    await context.Response.WriteAsync("<li><a href='/cookies'>Cookies</a></li><br>");
    await context.Response.WriteAsync("<li><a href='/querystring'>Query Strings</a></li><br>");
    await context.Response.WriteAsync("<li><a href='/hiddenfield'>Hidden Field</a></li>");
    await context.Response.WriteAsync("</ul>");
    await context.Response.WriteAsync("</body></html>");
});

app.MapGet("/session", async context =>
{
    var session = context.Session;
    session.SetString("Name", "Sudin Bohara");
    await context.Response.WriteAsync($"Session value set: Name = {session.GetString("Name")}");
});

app.MapGet("/httpcontext", async context =>
{
    var name = context.Request.HttpContext.Items["Name"] as string;
    context.Request.HttpContext.Items["Name"] = "Sudin Bohara";
    if (!string.IsNullOrEmpty(name))
    {
        await context.Response.WriteAsync($"HttpContext value retrieved: Name = {name}\n");
    }
    else
    {
        await context.Response.WriteAsync("No HttpContext value retrieved\n");
    }
    await context.Response.WriteAsync($"New HttpContext value set: Name = Sudin Bohara");
});


app.MapGet("/tempdata", async context =>
{
    var tempData = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>().GetTempData(context);
    tempData["Message"] = "This is Tempdata, I am Sudin";
    await context.Response.WriteAsync($"TempData value set and value is: {tempData["Message"]}");
});



app.MapGet("/cookies", async context =>
{
    var userIdCookie = context.Request.Cookies["UserID"];
    if (string.IsNullOrEmpty(userIdCookie))
    {
        Console.WriteLine("UserID cookie not found");
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("UserID cookie not found");
        return;
    }
    await context.Response.WriteAsync($"UserID cookie value: {7777}");
});



app.MapGet("/querystring", async context =>
{
    var name = context.Request.Query["name"];
    if (!string.IsNullOrEmpty(name))
    {
        await context.Response.WriteAsync($"Query String: Name = {name}");
    }
    else
    {
        name = "Sudin";
        await context.Response.WriteAsync($"Query string parameter 'name' not found. Using default value: {name}");
    }
});



app.MapGet("/hiddenfield", async context =>
{
    await context.Response.WriteAsync(@"
        <html>
        <body>
            <form method='post' action='/hiddenfield'>
                <input type='hidden' name='UserID' value='9999'>
                <button type='submit'>Submit</button>
            </form>
        </body>
        </html>
    ");
});

app.MapPost("/hiddenfield", async context =>
{
    var userId = context.Request.Form["UserID"];
    await context.Response.WriteAsync($"Hidden field value received: UserID = {userId}");
});

app.Run();