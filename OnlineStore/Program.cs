using OnlineStore.Interface;
using OnlineStore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<ITabs, InMemoryTabs>();
builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
builder.Services.AddScoped<ICurrentTime, UTCCurrentTime>();
builder.Services.AddHostedService<SendBackgroundService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//var phones = new Phones();

/* === Thread-safe collection === */
/*app.MapGet("/catalog/get_tabs", (ITabs tabs) => tabs.GetTabs());

app.MapPost("/catalog/add_tab", (ITabs tabs, Tab tab) => tabs.AddTabs(tab));

app.MapGet("/catalog/delete_tab", (ITabs tabs, int id) => tabs.DeleteTab(id));*/


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
app.MapGet("/email_sender", (IEmailSender sender, ICurrentTime utc) => 
    sender.Send(
    "PV011",
    "asp2022pd011@rodion-m.ru",
    "windows84@rambler.ru",
    "Server operation",
    "Local current time: " + 
    utc.GetCurrentTimeLocal() +
    ". <br>UTC current time: " + 
    utc.GetCurrentTimeUTC()
    ));

app.Run();