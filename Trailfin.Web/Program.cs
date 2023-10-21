using Microsoft.OpenApi.Models;
using Trailfin.Web.Controllers.Auth;
using Trailfin.Application;
using Trailfin.Application.Models.Helpers;
using Trailfin.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Tripmate API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
const string appCorsPolicy = "_myAllowSpecificOrigins";
builder.Services.AddControllers();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: appCorsPolicy, policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<JwtMiddleware>();
app.UseCors(appCorsPolicy);
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
public partial class Program { }