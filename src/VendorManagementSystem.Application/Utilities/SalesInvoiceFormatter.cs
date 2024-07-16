using System.Text;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;

namespace VendorManagementSystem.Application.Utilities
{
    internal static class SalesInvoiceFormatter
    {
        private static string _salesInvoiceSkeleton = @"<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Sales Invoice</title>
    <style>
      * {
        padding: 0;
        margin: 0;
        box-sizing: border-box;
      }
      .main {
        width: 90%;
        margin-left: auto;
        margin-right: auto;
        margin-top: 60px;
        border: 1px solid black;
      }
      .section-1 {
        width: 100%;
        display: flex;
        justify-content: space-between;
        padding: 8px 16px;
      }
      .logo-address {
        display: flex;
        justify-content: center;
      }
      .logo-address > img {
        width: 150px;
        height: 150px;
        margin-right: 24px;
      }
      .company-address > li:first-child {
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 12px;
      }
      .address {
        list-style: none;
        display: flex;
        flex-direction: column;
        justify-content: center;
        font-size: 14px;
      }
      .side-heading {
        text-align: right;
        align-self: flex-end;
        padding-bottom: 24px;
        font-family: ""Lucida Sans"", ""Lucida Sans Regular"", ""Lucida Grande"",
          ""Lucida Sans Unicode"", Geneva, Verdana, sans-serif;
        font-size: 12px;
        display: flex;
        flex-direction: column;
        gap: 4px;
      }
      .side-heading > span:first-child {
        letter-spacing: 2px;
        font-size: 32px;
        font-weight: bold;
      }
      .bold {
        font-weight: bold;
      }
      .section-2 {
        display: flex;
        border-top: 1px solid black;
      }
      .section-2-table-1 {
        /* flex-grow: 1; */
        width: 50%;
        border-right: 1px solid black;
        padding: 4px 8px;
      }
      .section-2-table-2 {
        /* flex-grow: 1; */
        width: 50%;
        padding: 4px 8px;
      }
      .section-3 {
        width: 100%;
        border-top: 1px solid black;
      }
      .section-3-table-1 {
        width: 100%;
      }
      .address-col-1 {
        flex-grow: 1;
      }
      .section-3-table {
        width: 100%;
        border-collapse: collapse;
      }

      .table-header,
      .table-data {
        border: 1px solid black;
        padding: 8px;
        text-align: left;
      }

      .half-width {
        width: 50%;
      }

      thead {
        background-color: #f2f2f2; /* Light grey background for the header */
      }
      .table-container {
        width: 100%;
        margin: 0 auto;
        border: 1px solid black;
      }

      .section-4-table {
        width: 100%;
        border-collapse: collapse;
      }

      .section-4-th,
      .section-4-td {
        border: 1px solid black;
        padding: 8px;
        text-align: left;
      }
      .sub-header {
        text-align: center;
        border-right: 1px solid black;
      }
      .section-5 {
        width: 100%;
        display: flex;
        margin-top: 20%;
        border-top: 1px solid black;
      }
      .sub-section-left,
      .sub-section-right {
        width: 50%;
        padding: 4px 12px;
      }
      .sub-section-left {
        border-right: 1px solid black;
        display: flex;
        flex-direction: column;
        justify-content: center;
      }

      .sub-section-right > ul {
        list-style: none;
      }
      .sub-section-right li {
        display: flex;
        text-align: right;
      }
      .sub-section-right li > span {
        width: 50%;
      }
      .sub-section-right {
        text-align: right;
      }
      .text-red {
        color: red;
      }
      .section-6 {
        border-top: 1px solid black;
        display: flex;
        flex-direction: column;
        gap: 10px;
        padding: 4px 12px;
        font-size: 12px;
      }

      .section-6 > ul {
        font-size: 12px;
      }
      .terms-and-conditions {
        display: flex;
        flex-direction: column;
      }
      .section-7 {
        margin-top: 20%;
        display: flex;
        flex-direction: column;
        justify-content: end;
        align-items: end;
        padding: 14px;
      }
    </style>
  </head>
  <body>
    <div class=""main"">
      <div class=""section-1"">
        <div class=""logo-address"">
          <img src = 'data:image/png;base64,{{IMAGE_BASE64}}' alt = 'EXSQ logo' />
          <ul class=""company-address address"">
            {{exsq_address}}
          </ul>
        </div>
        <div class=""side-heading"">
          <span>TAX INVOICE</span>
          <spanp>Invoice# <span class=""bold"">{{invoice_id}}</span></spanp>
        </div>
      </div>
      <div class=""section-2"">
        <table class=""section-2-table-1"">
          <tbody>
            <tr>
              <td>Invoice Date</td>
              <td>:<span class=""bold"">{{invoice_date}}</span></td>
            </tr>
            <tr>
              <td>Terms</td>
              <td>:<span class=""bold"">{{invoice_terms}}</span></td>
            </tr>
            <tr>
              <td>Due Date</td>
              <td>:<span class=""bold"">{{due_date}}</span></td>
            </tr>
          </tbody>
        </table>
        <table class=""section-2-table-2"">
          <tbody>
            <tr>
              <td>Place Of Supply</td>
              <td>:<span class=""bold"">{{place_of_supply}}</span></td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class=""section-3"">
        <table class=""section-3-table"">
          <thead>
            <tr>
              <th class=""half-width table-header"">Bill To</th>
              <th class=""half-width table-header"">Ship To</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td class=""half-width table-data"">
                <span class=""bold"">Attn</span> :
              </td>
              <td class=""half-width table-data"">
                <ul class=""address address-col-2"">
                  {{shipping_address}}
                </ul>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class=""section-4"">
        <table class=""section-4-table"">
          <thead>
            <tr>
              <th rowspan=""2"" class=""section-4-th"">#</th>
              <th rowspan=""2"" class=""section-4-th"">Item and Description</th>
              <th rowspan=""2"" class=""section-4-th"">HSN/SAC</th>
              <th rowspan=""2"" class=""section-4-th"">Qty</th>
              <th rowspan=""2"" class=""section-4-th"">Rate</th>
              <th colspan=""2"" class=""sub-header section-4-th"">CGST</th>
              <th colspan=""2"" class=""sub-header section-4-th"">SGST</th>
              <th rowspan=""2"" class=""section-4-th"">Amount</th>
            </tr>
            <tr>
              <th class=""sub-header"">%</th>
              <th class=""sub-header"">Amt</th>
              <th class=""sub-header"">%</th>
              <th class=""sub-header"">Amt</th>
            </tr>
          </thead>
          <tbody>
            {{item_rows}}
          </tbody>
        </table>
      </div>
      <div class=""section-5"">
        <div class=""sub-section-left"">
          <span>Total in Words</span>
          <span class=""bold"">{{amount_in_words}}</span>
        </div>
        <div class=""sub-section-right"">
          {{selling_summary}}
        </div>
      </div>
      <div class=""section-6"">
        <span>Notes</span>
        <span>Our Bank Account Details</span>
        <ul class=""address"">
          <li>Beneficiary Bank of USD: Citibank India</li>
          <li>SWIFT Code: CITIINBX</li>
          <li>Beneficiary a/c no: 0558996005</li>
          <li>Purpose of remittance in field 70: IT SERVICES</li>
          <li>Beneficiary A/C Name &Address:</li>
          <li>EX Squared Solutions India Private Limited</li>
          <li>Plot No 35, First Floor, Sector 27A</li>
          <li>Faridabad, Haryana, 121003</li>
        </ul>
        <span class=""terms-and-conditions"">
          <span>Terms & Conditions</span>
          <span
            >* SUPPLY MEANT FOR EXPORT UNDER LETTER OF UNDERTAKING WITHOUT
            PAYMENT OF INTEGRATED TAX</span
          >
        </span>
      </div>
      <div class=""section-7"">
        <span class=""bold"">For EX Squared Solutions India Pvt Ltd</span>
         <img src = 'data:image/png;base64,{{signature}}' alt = 'Signature' style=""height:100px; width:260px"" />
        <span>Authorized Signatory</span>
      </div>
    </div>
  </body>
</html>
";

        /*<li>EX Squared Solutions India Pvt Ltd</li>
            <li>Plot No 35, Sector 27-A</li>
            <li>Faridabad Haryana 121003 India</li>
            <li>Email : www.ex2india.com</li>
            <li>Website : sdhawan @ex2india.com</li>
            <li>CIN : U72300HR2015FTC057646</li>
            <li>PAN : AAECE1511H</li>
            <li>GSTIN : 06AAECE1511H1Z6</li>*/
        private static string _exSqAddress = @"<li>{0}</li>
            <li>{1}</li>
            <li>{2} {3} {4} {5}</li>
            <li>Email : www.ex2india.com</li>
            <li>Website : sdhawan@ex2india.com</li>
            <li>CIN : U72300HR2015FTC057646</li>
            <li>PAN : AAECE1511H</li>
            <li>GSTIN : {6}</li>";
        /*<li>
                    Aadesh Gulati, A-788, Dabua Colony, N.I.T Faridabad,
                    Landmark : Prakash Bal Bharti Public School
                  </li>
                  <li>Faridabad</li>
                  <li>Haryana</li>
                  <li>India</li>*/
        private static string _shippingAddress = @"<li>{0}</li>
                  <li>{1}</li>
                  <li>{2}</li>
                  <li>{3}</li>";
        /*<tr>
              <td class=""section-4-td"">1</td>
              <td class=""section-4-td"">Old Monitor(Dell S2216H)</td>
              <td class=""section-4-td"">84713010</td>
              <td class=""section-4-td"">2</td>
              <td class=""section-4-td"">2,118.64</td>
              <td class=""section-4-td"">9%</td>
              <td class=""section-4-td"">381.36</td>
              <td class=""section-4-td"">9%</td>
              <td class=""section-4-td"">381.36</td>
              <td class=""section-4-td"">4,237.28</td>
            </tr>*/
        private static string _itemRow = @"<tr>
              <td class=""section-4-td"">{0}</td>
              <td class=""section-4-td"">{1}</td>
              <td class=""section-4-td"">{2}</td>
              <td class=""section-4-td"">{3}</td>
              <td class=""section-4-td"">{4}</td>
              <td class=""section-4-td"">{5}</td>
              <td class=""section-4-td"">{6}</td>
              <td class=""section-4-td"">{7}</td>
              <td class=""section-4-td"">{8}</td>
              <td class=""section-4-td"">{9}</td>
            </tr>";
        /*<ul>
            <li>
              <span>Sub Total</span>
              <span>4,237.28</span>
            </li>
            <li>
              <span>CGST9 (9%)</span>
              <span>381.36</span>
            </li>
            <li>
              <span>SGST9(9%)</span>
              <span>381.36</span>
            </li>
            <li>
              <span class=""bold"">Total</span>
              <span class=""bold"">5,000</span>
            </li>
            <li>
              <span>Payment Made</span>
              <span class=""text-red"">(-) 5,000.00</span>
            </li>
            <li>
              <span class=""bold"">Balance Due</span>
              <span class=""bold"">0.00</span>
            </li>
          </ul>*/
        private static string _sellingSummary = @"<ul>
            <li>
              <span>Sub Total</span>
              <span>{0}</span>
            </li>
            <li>
              <span>CGST9 (9%)</span>
              <span>{1}</span>
            </li>
            <li>
              <span>SGST9 (9%)</span>
              <span>{2}</span>
            </li>
            <li>
              <span class=""bold"">Total</span>
              <span class=""bold"">{3}</span>
            </li>
            <li>
              <span>Payment Made</span>
              <span class=""text-red"">(-) {4}</span>
            </li>
            <li>
              <span class=""bold"">Balance Due</span>
              <span class=""bold"">{5}</span>
            </li>
          </ul>";

        private static decimal _totalCgst = -1;
        private static decimal _totalSgst = -1;
        private static decimal _subTotal = -1;
        private static decimal _amount = -1;

        public static string GetPdfContent(SalesInvoiceEmailDto salesInvoiceEmailDto, VendorNewResponseDto SellingVednor, VendorNewResponseDto BuyingVendor)
        {
            string exSqAddress = fomrateExSqAddress(SellingVednor);
            string shippingAddress = formateShippingAddress(BuyingVendor);
            string items = formateItemRows(salesInvoiceEmailDto.Items);
            string summary = formateSummary(salesInvoiceEmailDto.Items, salesInvoiceEmailDto.AmountPaid);
            string[] amountString = _amount.ToString().Split(".");
            int intergerValue = 0;
            int decimalValue = 0;
            if(amountString.Length == 1)
            {
                intergerValue = int.Parse(amountString[0]);
            }
            if (amountString.Length == 2)
            {
                intergerValue = int.Parse(amountString[0]);
                decimalValue = int.Parse(amountString[1]);
            }
            string amountInWords = $"Indian Rupee {GetWordFromNumber(intergerValue)} and {GetWordFromNumber(decimalValue)} Paise Only";
            return _salesInvoiceSkeleton.Replace("{{exsq_address}}", exSqAddress).Replace("{{invoice_id}}", salesInvoiceEmailDto.InvoiceId).Replace("{{invoice_date}}", salesInvoiceEmailDto.InvoiceDate.ToString()).Replace("{{invoice_terms}}", salesInvoiceEmailDto.Terms).Replace("{{due_date}}", salesInvoiceEmailDto.DueDate.ToString()).Replace("{{place_of_supply}}", salesInvoiceEmailDto.PlaceOfSupply).Replace("{{shipping_address}}", shippingAddress).Replace("{{item_rows}}", items).Replace("{{selling_summary}}", summary).Replace("{{amount_in_words}}", amountInWords);
        }

        private static string fomrateExSqAddress(VendorNewResponseDto sellingVendor)
        {
            var billingAddress = sellingVendor.BillingAddress??new AddressResponseDto();
            return string.Format(_exSqAddress, sellingVendor.CompanyName, billingAddress.AddressLine1, billingAddress.State, billingAddress.State, billingAddress.PinCode, billingAddress.Country, sellingVendor.GSTIN);
        }

        private static string formateShippingAddress(VendorNewResponseDto buyingVendor)
        {
            var shippingAddress = buyingVendor.ShippingAddress ?? new AddressResponseDto();
            return string.Format(_shippingAddress, shippingAddress.AddressLine1, shippingAddress.City, shippingAddress.State, shippingAddress.Country);
        }
        private static string formateItemRows(List<SaleItem> items)
        {
            StringBuilder rows = new StringBuilder();
            int counter = 0;
            _totalCgst = 0;
            _totalSgst = 0;
            foreach (SaleItem item in items)
            {
                counter++;
                
                decimal cgst = getTax(item.Amount, (decimal)item.CGST);
                decimal sgst = getTax(item.Amount, (decimal)item.SGST);
                _totalCgst += cgst;
                _totalSgst += sgst;
                string row = string.Format(_itemRow, counter, item.ItemName, item.HSN, item.Quantity, item.Rate, item.CGST, cgst, item.SGST, sgst, item.Amount);
                rows.Append(row);
            }
            return rows.ToString();
        }

        private static string formateSummary(List<SaleItem> items, decimal paid)
        {
            if (_totalCgst == -1) CalculateTotalCgst(items);
            if (_totalSgst == -1) CalculateTotalSgst(items);
            if (_subTotal == -1) CalculateSubTotal(items);
            _amount = _subTotal + _totalCgst + _totalSgst;
            _amount = decimal.Round(_amount, 2);
            Console.WriteLine("Paid Amount {0}", paid);
            decimal due = _amount - paid;
            return string.Format(_sellingSummary, _subTotal, _totalCgst, _totalSgst, _amount, paid, due);
        }

        private static decimal getTax(decimal rate, decimal CGST)
        {
            return (rate * CGST) / 100;
        }

        private static void CalculateTotalCgst(List<SaleItem> items)
        {
            _totalCgst = 0;
            foreach (SaleItem item in items)
            {
                _totalCgst += getTax(item.Rate, (decimal)item.CGST);
            }
        }
        private static void CalculateTotalSgst(List<SaleItem> items)
        {
            _totalSgst = 0;
            foreach (SaleItem item in items)
            {
                _totalSgst += getTax(item.Rate, (decimal)item.SGST);
            }
        }
        private static void CalculateSubTotal(List<SaleItem> items)
        {
            _subTotal = 0;
            foreach (SaleItem item in items)
            {
                _subTotal += item.Amount;
            }
        }

       private static string GetWordFromNumber(int number)
        {
            string[] unitsMap = new string[] {"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"};
            string[] tensMap = new string[] {"Zero", "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"};

            StringBuilder wordBuilder = new StringBuilder();
            const int CRORE = 10000000;
            const int LAKHS = 100000;
            const int THOUSAND = 1000;
            const int HUNDREAD = 100;

            if((number / CRORE) > 0)
            {
                wordBuilder.Append(GetWordFromNumber(number / CRORE)).Append(" Crore ");
                number = number % CRORE;
            }

            if((number / LAKHS) > 0)
            {
                wordBuilder.Append(GetWordFromNumber(number / LAKHS)).Append(" Lakhs ");
                number = number % LAKHS;
            }

            if((number / THOUSAND) > 0)
            {
                wordBuilder.Append(GetWordFromNumber(number / THOUSAND)).Append(" Thousand ");
                number = number % THOUSAND;
            }

            if((number / HUNDREAD) > 0)
            {
                wordBuilder.Append(GetWordFromNumber(number / HUNDREAD)).Append(" Hundread ");
                number = number % HUNDREAD;
            }
            if (number > 0)
            {
                if(wordBuilder.Length > 0)
                     wordBuilder.Append(" and ");
                if (number < 20)
                {
                    wordBuilder.Append(unitsMap[number]);
                }
                else
                {
                    wordBuilder.Append(tensMap[number / 10]);
                    wordBuilder.Append("-").Append(unitsMap[number % 10]);
                }
            }
            return wordBuilder.ToString().Trim();
        }

    }
}
