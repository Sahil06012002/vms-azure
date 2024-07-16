using System.ComponentModel;

namespace VendorManagementSystem.Models.Enums
{
    public enum Salutation
    {
        Mr,
        Mrs,
        Ms,
        Miss
    }

    public enum Currency
    {
        INR,
        USD
    }

    public enum PaymentTerms
    {
        PrePaid,
        Due_end_of_the_month,
        Due_end_of_the_next_month
    }

    public enum AddressTypes
    {
        Billing,
        Shipping
    }

    public enum Country
    {
        India,
        USA
    }
    public enum Units
    {
        Cm,
        Kg
    }
    public enum TaxPreference
    {
        Taxable,
        Non_Taxable,
        Out_of_Scope,
        Non_GST_Supply
    }
    public enum ItemType
    {
        Goods,
        Services
    }
    public enum PurchaseStatus
    {
        Draft,
        Cancelled,
        Issued
    }

    public enum GST
    {
        Exempt = 0,
        LowRate = 5,
        MediumRate = 12,
        StandardRate = 18,
        HighRate = 28
    }

}
