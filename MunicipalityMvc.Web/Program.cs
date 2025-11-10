using MunicipalityMvc.Core.Services;
using MunicipalityMvc.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (MVC controllers + views)
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure app data directory and register services
var appData = Path.Combine(builder.Environment.ContentRootPath, "AppData");
builder.Services.AddSingleton<IIssueService>(_ => new IssueService(appData));
builder.Services.AddSingleton<IEventsService>(_ => new EventsService(appData));
builder.Services.AddSingleton<IServiceRequestStatusService, ServiceRequestStatusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
