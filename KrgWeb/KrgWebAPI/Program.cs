using DBLayer.Models;
using DBLayer.Service;
using DBLayer.Service.Authentication;
using KrgWebAPI.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Fethcing Connection string from appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


////////////////// SeriLog Configuration////////////////////////
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Add(StandardColumn.LogEvent);

Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Async(a => a.MSSqlServer(
        connectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = false },
        restrictedToMinimumLevel: LogEventLevel.Error,
        columnOptions: columnOptions
    ))
    .WriteTo.Async(a => a.File(
        path: "logs/fallback.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Error
    ))
    .CreateLogger();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://192.168.0.9:8002", "http://localhost:4200", "https://192.168.0.9:96") // your Angular app's origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              // Content-Disposition is not CORS-safelisted, so the invoice PDF download
              // cannot read the filename the API sends without this.
              .WithExposedHeaders("Content-Disposition")
              .AllowCredentials();
    });
});


builder.Host.UseSerilog();

//////////////////// JWT Validation ///////////////////////////
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies[HeaderConstants.JwtCookie];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token; // Assign token from cookie to context for validation
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<NsinvoiceBillingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();
builder.Services.AddScoped<IRecentActivityService, RecentActivityService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceReportService, InvoiceReportService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICustomerPersistentQueueService, CustomerPersistentQueueService>();
builder.Services.AddScoped<ICustomerCacheService, CustomerCacheService>();

///////////////////////////// Profilers /////////////////////////////////////////////////////////



// Add services to the container.

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<KrgWebAPI.SecurityAndExceptionMiddleware>();

app.MapControllers();

app.Run();
