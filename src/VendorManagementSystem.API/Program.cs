using System.Text.Json.Serialization;
using VendorManagementSystem.Application;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Infrastructure;
using Microsoft.OpenApi.Models;
namespace VendorManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    }).AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        options.JsonSerializerOptions.IncludeFields = true;

                    });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddApplication(builder.Configuration)
                .AddInfrastructer(builder.Configuration);

            IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettingsDto>(jwtSettings);

            IConfigurationSection emailSettings = builder.Configuration.GetSection("Email");
            builder.Services.Configure<EmailSettingsDto>(emailSettings);

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Vendor Management System API", Version = "v1" });
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

            var configuration = builder.Configuration;
            /*string kVURL = configuration["KeyVaultConfig:kvurl"]!;
            string tenantid = configuration["KeyVaultConfig:TenantId"]!;
            string clientid = configuration["KeyVaultConfig:ClientId"]!;
            string clientSecret = configuration["KeyVaultConfig:ClientsecretId"]!;
            var credentials = new ClientSecretCredential(tenantid, clientid, clientSecret);
            var client = new SecretClient(new Uri(kVURL), credentials);
            builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());


            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            
        }
    }
}
