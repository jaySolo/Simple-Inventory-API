using System.Diagnostics;
using System.Reflection;

using Microsoft.OpenApi.Models;

using jsolo.simpleinventory.web.Configurations;
using jsolo.simpleinventory.web;



var builder = WebApplication.CreateBuilder(args);

Assembly? assembly = Assembly.GetEntryAssembly();
FileVersionInfo appVersionInfo = FileVersionInfo.GetVersionInfo(assembly?.Location ?? string.Empty);

// Add services to the container.
#region services registration

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(desc => desc.First());
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jsolo.simpleinventory.web.api",
        Version = "0.1.0.0",
        Description = "Web API for SimpleInventory by Jamal J. A. Solomon",
        // TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Jamal J. A. Solomon",
            Email = "jj_solomon16@live.com",
            // Url = new Uri("https://twitter.com/jwalkner"),
        },
        License = new OpenApiLicense
        {
            Name = "The Unicense",
            Url = new Uri("https://unlicense.org"),
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddHealthChecks();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

// builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// builder.Services.AddSystem().AddInfrastructure(builder.Configuration);

// // configure jwt authentication
// var appSettings = appSettingsSection.Get<AppSettings>();
// var key = Encoding.ASCII.GetBytes(appSettings.Secret);
// builder.Services.AddAuthentication(x =>
// {
//     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(x =>
// {
//     x.RequireHttpsMetadata = false;
//     x.SaveToken = true;
//     x.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(key),
//         ValidateIssuer = false,
//         ValidateAudience = false
//     };
// });


// // configure DI for application services
// builder.Services.AddScoped<IUserService, UserService>();

// builder.Services.AddAuthorization(opts =>
// {
//     opts.AddPolicy(Policies.AllowDevelopers, policy => policy.RequireClaim(
//         Claims.Developer.Type,
//         Claims.Developer.Value
//     ));

//     opts.AddPolicy(Policies.AllowSystemAdmins, policy => policy.RequireClaim(
//         Claims.Administrator.Type,
//         Claims.Administrator.Value
//     ));

//     opts.AddPolicy(Policies.AllowAuthenticatedUsers, policy => policy.RequireAuthenticatedUser());
// });
#endregion


var app = builder.Build();


#region application configuration

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "docs/{documentname}.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/docs/v1.json", name: "Version 1.0.0");
        c.RoutePrefix = "docs";
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseHealthChecks("/system/health");

// global cors policy
app.UseCors(opts => opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

#region Routes Definitions

app.MapGet("/system/about", async context =>
{
    var appInfo = assembly?.GetName();
    
    context.Response.Headers.Append("Accept", "application/json");
    context.Response.Headers.Append("Content-Type", "application/json");

    await context.Response.Body.WriteAsync(System.Text.Json.JsonSerializer
        .SerializeToUtf8Bytes(new
        {
            Name = appVersionInfo.ProductName ?? "APUA Electricity BU R.E. Customers Tracker",
            ProductVersion = appVersionInfo?.ProductVersion ?? "",
            AssemblyVersion = appInfo?.Version?.ToString() ?? "",
            Copyright = appVersionInfo?.LegalCopyright ?? "",
            Company = appVersionInfo?.CompanyName ?? ""
        })
    );
})
.WithName("GetSystemInfo")
.WithOpenApi();

app.MapGet("/system", async context =>
{
    await context.Response.Body.WriteAsync(System.Text.Encoding.ASCII.GetBytes(
        $"{appVersionInfo?.ProductName ?? "App"} is running."
    ));
})
.WithName("GetSystemState")
.WithOpenApi();

app.MapControllers();
#endregion

#endregion


bool should_print_to_console = Environment.UserInteractive && !Console.IsInputRedirected,
    should_migrate_db = STARTUP_OPTIONS_CHECKER.MIGRATE_DATABASE(args),
    should_seed_db = STARTUP_OPTIONS_CHECKER.ATTEMPT_SEED_DATABASE(args, should_migrate_db);


#region database migration
#endregion


if (STARTUP_OPTIONS_CHECKER.LAUNCH_APP(args, should_migrate_db))
{
    app.Run();
}
else if (should_print_to_console)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}
