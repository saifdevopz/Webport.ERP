using BlazorDashboard.Common.Authentication;
using BlazorDashboard.Common.HttpClients;
using BlazorDashboard.Common.Services.Implementations;
using BlazorDashboard.Common.Services.Interfaces;
using BlazorDashboard.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Http Client
builder.Services.AddHttpClient<BaseHttpClient>((sp, client) =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["BaseUrls:Production"]!);
});

builder.Services.AddHttpClient<TenantHttpClient>((sp, client) =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["Tenant:BaseUrl"]!);
});

builder.Services.AddScoped<BaseHttpClient>();
builder.Services.AddScoped<TenantHttpClient>();

// Services
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

await app.RunAsync();
