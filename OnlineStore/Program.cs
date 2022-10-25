using OnlineStore.Configuration;
using OnlineStore.Interface;
using OnlineStore.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(
        (ctx, lc) =>
        lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration)
    );
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<SmtpConfig>(
        builder.Configuration.GetSection("SmtpConfig")
    );

    //builder.Services.AddSingleton<ITabs, Tabs>();
    builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
    builder.Services.AddSingleton<IClock, Clock>();
    builder.Services.AddHostedService<SendBackgroundService>();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSerilogRequestLogging();

    //var phones = new Phones();

    /* === Thread-safe collection === */
    /*app.MapGet("/catalog/get_tabs", (ITabs tabs) =>
     tabs.GetTabs());

    app.MapPost("/catalog/add_tab", (ITabs tabs, Tab tab) => 
    tabs.AddTabs(tab));

    app.MapGet("/catalog/delete_tab", (ITabs tabs, int id) => 
    tabs.DeleteTab(id));*/


    /* === Lock === */
    /*app.MapGet("/catalog/get_phones", () => phones.GetPhones());

    app.MapPost("/catalog/add_phone", (Phone phone, HttpContext context) =>
    {
        phones.AddPhone(phone);
        context.Response.StatusCode = 201;
    });

    app.MapPost("catalog/clear_phones", (HttpContext context) =>
    {
        phones.ClearPhones();
        context.Response.StatusCode = 205;
    });*/

    /* === EmailService and CurrentTime === */
    async Task<string> SendMail(
        IEmailSender sender,
        IClock clock,
        string fromName,
        string to,
        string subject,
        string body
    )
    {
        await sender.SendAsync(fromName, to, subject, body);
        return "Ok. Time " + clock.GetCurrentTimeUTC();
    }

    app.MapGet("/email_sender", SendMail);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}