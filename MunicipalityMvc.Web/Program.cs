using MunicipalityMvc.Core.Services;
using MunicipalityMvc.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (MVC controllers + views)
builder.Services.AddControllersWithViews();

// Configure app data directory and register the queue-backed issue service
var appData = Path.Combine(builder.Environment.ContentRootPath, "AppData");
builder.Services.AddSingleton<IIssueService>(_ => new IssueService(appData));

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

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
