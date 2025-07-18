using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TodoImail.BlazorApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient(sp => new HttpClient() { BaseAddress = new Uri("http://localhost:5282") });
builder.Services.AddSingleton<ITodoImailClientService, TodoImailClientService>();


await builder.Build().RunAsync();
