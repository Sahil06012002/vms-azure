using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PdfSharp.Fonts;
using System.Text;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Services;
using VendorManagementSystem.Application.Utilities;

namespace VendorManagementSystem.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("in application di");
            /*var valassembly = typeof(DependencyInjectionExtensions).Assembly;
            services.AddValidatorsFromAssembly(valassembly);*/

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVendorService, VendorService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUtilityService, UtilityService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();
            services.AddTransient<ISalesInvoiceService, SalesInvoiceService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IExpenditureService, ExpenditureService>();
            services.AddTransient<IDigitalSign,DigitalSign>();
            return services;
        }
    }
}
