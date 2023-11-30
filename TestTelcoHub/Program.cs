using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.OpenApi.Models;
using TestTelcoHub.Infastruture.Lib;
using TestTelcoHub.Model.Helper;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Service.Interface;
using TestTelcoHub.Service.Service;
using TestTelcoHub.Service.ServiceExtension;
using static TestTelcoHub.Model.Model.Plan;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDIService(builder.Configuration);
builder.Services.AddScoped<IValidator<Contacts>, ContactsValidator>();
builder.Services.AddScoped<IValidator<Address>, AddressValidator>();
builder.Services.AddScoped<IValidator<Distributor>, DistributorValidator>();
builder.Services.AddScoped<IValidator<Plan>, PlanValidator>();

builder.Services.AddScoped<IAutoUpdateService, AutoUpdateService>();
builder.Services.AddScoped<IBillingPlanService, BillingPlanService>();
builder.Services.AddScoped<IPurchaseHistoryService, PurchaseHistoryService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILogService,LogService>();
builder.Services.AddScoped<IApprovalCodeService, ApprovalCodeService>();

builder.Services.AddHostedService<AutoUpdateService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 


builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ops =>
{
    ops.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    ops.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme= "Bearer"
    });
    ops.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string []{ }
        }
    });
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
