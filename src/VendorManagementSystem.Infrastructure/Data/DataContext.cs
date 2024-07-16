using Microsoft.EntityFrameworkCore;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<VendorNew> VendorsNew { get; set; }
        public DbSet<PrimaryContact> PrimaryContact { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Expenditure> Expenditure { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<VendorCategoryMapping> VendorCategoryMappings { get; set; }
        public DbSet<ContractStatus> ContractStatus { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public DbSet <Invoice> Invoices { get; set; }
        public DbSet <PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet <Item> Products { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<VendorTdsOption> VendorTDSOptions { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchasedItem> PurchasedItem { get; set; }
        public DbSet <SalesInvoice> SalesInvoice { get; set; }
        public DbSet<SalesInvoiceItems> SalesInvoiceItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendorType>().HasData(
                new VendorType { Id = 1, Name = "New Vendors" },
                new VendorType { Id = 2, Name = "Completed Vendors" },
                new VendorType { Id = 3, Name = "In Progress" },
                new VendorType { Id = 4, Name = "On Hold" },
                new VendorType { Id = 5, Name = "Need Follow Up" }
            );

            modelBuilder.Entity<ContractStatus>().HasData(
                new ContractStatus { Id = 1, Name = "Active" },
                new ContractStatus { Id = 2, Name = "Expired" },
                new ContractStatus { Id = 3, Name = "Pending" }
            );

            modelBuilder.Entity<Contract>()
                .Property(e => e.StartDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v)
                );

            modelBuilder.Entity<Contract>()
               .Property(e => e.EndDate)
               .HasConversion(
                   v => v.ToDateTime(TimeOnly.MinValue),
                   v => DateOnly.FromDateTime(v)
               );

            modelBuilder.Entity<Invoice>()
               .Property(e => e.DueDate)
               .HasConversion(
                   v => v.ToDateTime(TimeOnly.MinValue),
                   v => DateOnly.FromDateTime(v)
               );
            
            /*modelBuilder.Entity<PurchaseOrder>()
            .Property(e => e.Date)
            .HasConversion(
             v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : default,
             v => v.HasValue ? DateOnly.FromDateTime(v) : default
                );*/


            modelBuilder.Entity<VendorNew>()
                .Property(v => v.PaymentTerms)
                .HasConversion<string>();

            modelBuilder.Entity<VendorNew>()
               .Property(v => v.Currency)
               .HasConversion<string>();

            modelBuilder.Entity<Address>()
               .Property(v => v.AddressType)
               .HasConversion<string>();

            modelBuilder.Entity<Address>()
               .Property(v => v.Country)
               .HasConversion<string>();

            modelBuilder.Entity<PrimaryContact>()
               .Property(v => v.Salutation)
               .HasConversion<string>();

            modelBuilder.Entity<UserToken>()
                .HasKey(ut => new { ut.Email });
            
            modelBuilder.Entity<Item>()
                .Property(i => i.ItemType)
                .HasConversion<string>();

            modelBuilder.Entity<Item>()
                .Property(i => i.TaxPreference)
                .HasConversion<string>();

            modelBuilder.Entity<PurchaseOrder>()
                .Property(i => i.PurchaseStatus)
                .HasConversion<string>();

            modelBuilder.Entity<PurchaseOrder>()
                .Property(i => i.PaymentTerms)
                .HasConversion<string>();

            modelBuilder.Entity<SalesInvoice>()
               .Property(i => i.Status)
               .HasConversion<string>();

            modelBuilder.Entity<SalesInvoice>()
                .Property(i => i.PaymentTerms)
                .HasConversion<string>();
        }
    }
}
