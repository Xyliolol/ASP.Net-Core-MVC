using MVCApp.Interface;
using MVCApp.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
.AddUserSecrets<EmailSendlerCredentials>(true)
.Build();
try
{
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddScoped<IEmailSender, EmailSender>();  
    builder.Services.AddSingleton<IConfigurationRoot>(config);
    builder.Services.Configure<SmtpCredentials>(builder.Configuration.GetSection("SmtpCredentials"));

    builder.Host.UseSerilog((_, conf) =>
    {
        conf
      .WriteTo.Console()
      .WriteTo.File("log.txt");
    });

    var app = builder.Build();



    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseSerilogRequestLogging();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server down");
}
finally
{
    Log.Information("shut down complete");
    Log.CloseAndFlush();
}