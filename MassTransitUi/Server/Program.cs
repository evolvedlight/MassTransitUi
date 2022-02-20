using MassTransitUi.Server;
using MassTransitUi.Server.Data;
using MassTransitUi.Server.Hubs;
using MassTransitUi.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MassTransitSettings>(builder.Configuration.GetSection(nameof(MassTransitSettings)));

builder.Services.AddDbContext<MassTransitUiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("InternalDatabase")));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
  opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
      new[] { "application/octet-stream" });
});

builder.Services.AddSingleton<IErrorPipelineService, ErrorPipelineService>();
builder.Services.AddSingleton<IManagementApiService, ManagementApiService>();
builder.Services.AddSingleton<RabbitMessageOutgoingService>();

builder.Services.AddHostedService<RabbitErrorQueuesMonitor>();

var app = builder.Build();
app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapHub<ErrorQueueHub>("/error-queue-hub");
app.MapFallbackToFile("index.html");

app.Run();
