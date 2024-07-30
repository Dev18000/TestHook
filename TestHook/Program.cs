using TestHook.Data;
using TestHook.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllers();

builder.Services.AddScoped<IClientSubscriptionService, ClientSubscriptionService>();

// HTTP client handler for custom certificate validation (if needed)
var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

builder.Services.AddHttpClient();

// Add SignalR
builder.Services.AddSignalR();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAllOrigins");
app.MapControllers();
app.MapBlazorHub();
app.MapHub<PlanningHub>("/planninghub");
app.MapFallbackToPage("/_Host");

app.Run();
