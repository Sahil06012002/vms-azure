using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Infrastructure.Repository;
using VendorManagementSystem.Infrastructure.Services;

namespace VendorManagementSystem.Infrastructure
{
    public static class InfrastructerDependencyInjection
    {
        public static IServiceCollection AddInfrastructer(this IServiceCollection services, IConfiguration configuration)
        {
            //local keyvault
            /*string kVURL = configuration["KeyVaultConfig:kvurl"]!;
            string tenantid = configuration["KeyVaultConfig:TenantId"]!;
            string clientid = configuration["KeyVaultConfig:ClientId"]!;
            string clientSecret = configuration["KeyVaultConfig:ClientsecretId"]!;
            var credentials = new ClientSecretCredential(tenantid, clientid, clientSecret);
            services.AddSingleton(x => new SecretClient(new Uri(kVURL), credentials));*/


           /* services.AddSingleton<IKeyVaultService, KeyVaultService>();

            //official keyvault
            string kVURL = configuration["KeyVaultConfig:kvurl"]!;
            services.AddSingleton(x => new SecretClient(new Uri(kVURL), new DefaultAzureCredential()));*/


            //getiing the key vault service instantly
            /*var keyVaultService = services.BuildServiceProvider().GetRequiredService<IKeyVaultService>();
            var dbEndpoint = keyVaultService.GetSecret("db-endpoint");
            var dbuser = keyVaultService.GetSecret("db-user");
            var dbpassword = keyVaultService.GetSecret("db-password");
            var fakeDbname = keyVaultService.GetSecret("db-name");
            string dbname = fakeDbname.Data.Replace("_", "");
            var dbConnection = $"Server={dbEndpoint.Data};DataBase={dbname};User Id={dbuser.Data};Password={dbpassword.Data};";
            services.AddDbContext<DataContext>(options => options.UseMySQL(dbConnection));*/

            services.AddDbContext<DataContext>(options => options.UseMySQL("Server=vms.mysql.database.azure.com;DataBase=vmsdb;User Id=vms_admin;Password=eBrhGsN3wjM3ptGS7USRYlpLSR;"));
            //services.AddDbContext<DataContext>(options => options.UseSqlServer(dbConnectionString, o => o.UseCompatibilityLevel(120)));
            services.AddSingleton(x => new BlobServiceClient(new Uri(configuration.GetConnectionString("Azure")), new DefaultAzureCredential()));

            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddScoped<IUserRepository, UserReopsitory>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRespository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IVendorCategoryRepository, VendorCategoryRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchasedItemRepository, PurchasedItemRepository>();
            services.AddScoped<IUtilityRespository, UtilityRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();    
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IExpenditureRepository, ExpenditureRepository>();
            services.AddTransient<ISalesInvoiceRepository, SalesInvoiceRepository>();
            services.AddTransient<ISelectedItemsRepository, SelectedItemsRepository>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IErrorLoggingService, ErrorLoggingService>();
            return services;
        }
    }
}
