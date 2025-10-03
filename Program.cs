var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () =>
{
    return "Say Hello World !!";
})
.WithName("HelloWorld");

app.MapGet("/home", () =>
{
    var html = @"
        <html>
            <head>
                <title>Simple API Home</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 40px; background: #f4f4f4; }
                    .container { background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px #ccc; }
                    h1 { color: #333; }
                    p { color: #555; }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Welcome to Simple API!</h1>
                    <p>This is a nice home page for your API.</p>
                    <ul>
                        <li><a href=""/weatherforecast"">Weather Forecast</a></li>
                        <li><a href=""/"">Hello World</a></li>
                    </ul>
                </div>
            </body>
        </html>";
    return Results.Content(html, "text/html");
})
.WithName("HomePage");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
