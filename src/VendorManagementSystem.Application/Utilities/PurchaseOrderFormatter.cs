using System.Text;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;

namespace VendorManagementSystem.Application.Utilities
{
    internal static class PurchaseOrderFormatter
    {

        private static string _purchaseOrderSkeleton = @"<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <style>
      * {
        padding: 0;
        margin: 0;
      }
      body {
        font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;
        padding: 14px;
        padding-left: 62px;
        padding-top: 62px;
        padding-right: 31px;
      }
      .top-section {
        display: flex;
        flex-direction: row;
        width: 100%;
      }
      .company-header {
        flex: 1;
        font-size: 12px;
      }
      ul {
        list-style: none;
      }
      .company-header li:first-child {
        font-weight: bold;
      }
      .company-header > img {
        width: 200px;
        height: 200px;
      }
      .po-header {
        flex: 1;
        text-align: right;
      }
      .po-header > h4 {
        font-size: 28px;
        font-weight: 500;
      }
      .po-header > h5 {
        font-size: 12px;
        font-weight: 600;
      }

      .vendor-address {
        margin-top: 28px;
        font-size: 12px;
      }

      .vendor-address li:nth-child(2) {
        font-weight: bold;
      }

      .delivery-address {
        margin-top: 28px;
        font-size: 12px;
      }

      .item-table {
        margin-top: 28px;
        width: 100%;
        border-collapse: collapse;
      }

      .item-table-header,
      .item-table-body {
        padding: 8px;
        text-align: left;
      }

      .item-table,
      .item-table-header,
      .item-table-body {
        border-bottom: 2px solid #999;
      }

      .item-table-header {
        background-color: #333;
        color: white;
      }

      .extra-table {
        width: fit-content;
        margin-left: auto;
      }
      .extra-table-data {
        text-align: right;
        padding-left: 60px;
        padding-top: 10px;
        padding-bottom: 10px;
      }
      .text-align-left {
        text-align: left;
      }
      .text-align-right {
        text-align: right;
      }
      .signature {
        margin-top: 100px;
        display: flex;
        flex-direction: row;
      }
    </style>
  </head>
  <body>
    <div class='top-section'>
      <div class='company-header'>
        <img src = 'data:image/png;base64,{{IMAGE_BASE64}}' alt = 'EXSQ logo' />
        <ul>
          {{exsq_address}}
        </ul>
      </div>
      <div class='po-header'>
        <h4>PURCHASE ORDER</h4>
        <h5>{{poId}}</h5>
      </div>
    </div>
    <ul class='vendor-address'>
      {{vendor_address}}
    </ul>

    <ul class='delivery-address'>
      <li>Deliver To</li>
      {{customer_address}}
    </ul>

    <table class='item-table'>
      <thead>
        <th class='item-table-header text-align-left'>#</th>
        <th class='item-table-header text-align-left'>Item & Description</th>
        <th class='item-table-header text-align-right'>Qty</th>
        <th class='item-table-header text-align-right'>Rate</th>
        <th class='item-table-header text-align-right'>Amount</th>
      </thead>
      <tbody>
        {{table_rows}}
      </tbody>
    </table>

    <table class='extra-table'>
      <tbody>
        {{extras}}
      </tbody>
    </table>

    <div class='signature'>
      <p>Authorized Signature</p>
      <p>___________________________________</p>
    </div>
  </body>
</html>
";

        private static string _exsqAddress = @"<li>{0}</li>
          <li>Attn: {0}</li>
      <li>{1}</li>
      <li>{2}</li>
      <li>{3}</li>
      <li>{4} {5}</li>
      <li>{6}</li>
      <li>GSTIN {7}</li>";

        private static string _deliveryFromAddress = @"<li>Vendor Address</li>
      <li>Attn: {0}</li>
      <li>{1}</li>
      <li>{2}</li>
      <li>{3}</li>
      <li>{4} {5}</li>
      <li>{6}</li>
      <li>GSTIN {7}</li>";

        private static string _customerAddress = @"<li>Vendor Address</li>
      <li>Attn: {0}</li>
      <li>{1}</li>
      <li>{2}</li>
      <li>{3}</li>
      <li>{4} {5}</li>
      <li>{6}</li>
      <li>GSTIN {7}</li>";



        private static string _tableRow = @"<tr>
          <td class='item-table-body'>{0}</td>
          <td class='item-data item-table-body'>{1}</td>
          <td class='item-table-body text-align-right'>{2}</td>
          <td class='item-table-body text-align-right'>{3}</td>
          <td class='item-table-body text-align-right'>{4}</td>
        </tr>";

        private static string _extras = @"<tr>
          <td class='extra-table-data'>Sub Total</td>
          <td class='extra-table-data text-align-right'>{0}</td>
        </tr>
        <tr>
          <td class='extra-table-data'>GST(18%)</td>
          <td class='extra-table-data text-align-right'>{1}</td>
        </tr>
        <tr>
          <td class='extra-table-data'>Total</td>
          <td class='extra-table-data text-align-right'>{2}</td>
        </tr>";
        public static string getPdfContent(PdfGenerationDto generationDto, VendorNewResponseDto vendor, VendorNewResponseDto creator, VendorNewResponseDto customer)
        {
            string exsqAddress = formateExsqAddress(creator);
            string vendorAddress = formateVendorAddress(vendor);
            string customerAddress = formatCustomerAddress(customer);
            string tableData = formateTableRows(generationDto.Rows);
            string extrasData = formateExtras(generationDto);
            return _purchaseOrderSkeleton.Replace("{{poId}}",generationDto.PurchaseOrderId).Replace("{{exsq_address}}", exsqAddress).Replace("{{vendor_address}}", vendorAddress).Replace("{{customer_address}}", customerAddress).Replace("{{table_rows}}", tableData).Replace("{{extras}}", extrasData);
        }


        public static string formateExsqAddress(VendorNewResponseDto creator)
        {
            var billingAddress = creator.BillingAddress ?? new AddressResponseDto();

            return String.Format(_exsqAddress, creator.CompanyName, billingAddress.Attention, billingAddress.AddressLine1 ?? billingAddress.AddressLine2, billingAddress.City, billingAddress.State, billingAddress.PinCode, billingAddress.Country, creator.GSTIN);
        }
        public static string formateVendorAddress(VendorNewResponseDto vendor)
        {
            var billingAddress = vendor.BillingAddress ?? new AddressResponseDto();
            return String.Format(_deliveryFromAddress, vendor.CompanyName, billingAddress.Attention, billingAddress.AddressLine1 ?? billingAddress.AddressLine2, billingAddress.City, billingAddress.State, billingAddress.PinCode, billingAddress.Country, vendor.GSTIN);
        }
        public static string formatCustomerAddress(VendorNewResponseDto customer)
        {
            var billingAddress = customer.BillingAddress ?? new AddressResponseDto();
            return String.Format(_customerAddress, customer.CompanyName, billingAddress.Attention, billingAddress.AddressLine1 ?? billingAddress.AddressLine2, billingAddress.City, billingAddress.State, billingAddress.PinCode, billingAddress.Country, customer.GSTIN);
        }

        public static string formateTableRows(List<ItemsRow> rows)
        {
            StringBuilder formatter = new StringBuilder();
            var row_number = 0;
            foreach (ItemsRow row in rows)
            {
                row_number++;
                formatter.Append(String.Format(_tableRow, Convert.ToString(row_number), row.ItemAndDescription, Convert.ToString(row.Quantity), Convert.ToString(row.Rate), Convert.ToString(row.Amount)));
            }
            return formatter.ToString();
        }

        public static string formateExtras(PdfGenerationDto generationDto)
        {
            decimal subTotal = getSubTotal(generationDto.Rows);
            decimal gst = (subTotal * generationDto.GST)/100;
            return String.Format(_extras, subTotal, gst, subTotal + gst);
        }

        private static decimal getSubTotal(List<ItemsRow> rows)
        {
            decimal subtotal = 0;
            foreach (ItemsRow row in rows)
            {
                subtotal += row.Amount;
            }
            return subtotal;
        }
    }
}
