using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SAPB1WordPressAPI.DataModel.DAL;
using SAPB1WordPressAPI.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPB1WordPressAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnersController : BaseController
    {
        private readonly SapDbContext sapDbContext;

        public BusinessPartnersController(SapDbContext myDbContext
            ) : base(myDbContext)
        {
            sapDbContext = myDbContext;
        }

        // GET: api/GetBusinessPartnersCreditLimitS/
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<OCRD>>> GetBusinessPartnersCreditLimit()
        {
            return await sapDbContext.OCRD.AsNoTracking().ToListAsync();
        }

        [HttpGet("[action]/cardCode")]
        public ActionResult<OCRD> GetBusinessPartnersCreditLimitByCardCode(string cardCode)
        {
            return sapDbContext.OCRD.AsNoTracking().Where(x => x.CardCode == cardCode).FirstOrDefault();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GenerateCustomerStatement(string cardCode , DateTime startDate, DateTime endDate)
        {
            //var cardCode = "129564";
            //var startDate = new DateTime(2018, 01, 01);
            //var endDate = new DateTime(2022, 12, 31);

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(await GetHTMLBodyAsync(cardCode, startDate, endDate));
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });

            return File(pdfContent, "application/pdf", $"{cardCode} - customerstatement.pdf");
        }

        private async Task<string> GetHTMLBodyAsync(string cardCode, DateTime startDate, DateTime endDate)
        {
            var details = await sapDbContext.GetCustomerStatementAsync(cardCode, startDate, endDate);
            string rows = "";
            string cardName = "";
            string totalRemainingPayment = "";

            if (details.Count > 0)
            {
                cardName = details.First().CustomerName;
                totalRemainingPayment = Math.Round((decimal)details.Sum(x => x.RemainingPayment), 2).ToString();
            }

            foreach (var dt in details)
            {

                rows += $"<tr>      <td> {dt.InvoicePostingDate.ToShortDateString()} </td>      <td> {dt.InvDocumentNumber} </td>      <td>$ {Math.Round((decimal)dt.DocTotal, 2)} </td>      <td>$ {Math.Round((decimal)dt.PaidToInvoice, 2)} </td>      <td> {dt.PaymentDocumentNumber}</td>      <td> {dt.PaymentPostingDate.ToShortDateString()} </td>      <td>$ {Math.Round((decimal)dt.RemainingPayment, 2)} </td>    </tr>";
            }

            string html = "<div>  <h1 style='text-align: center;'>Customer Statement</h1>  <h3>" + cardCode + " - " + cardName + "</h3>  <p>Generated from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString() + "</p>  <style>    th {      border-bottom: 2px solid black;    }  </style>  <table style='width: 100%'>    <!-- Inicio de encabezado -->    <tr>      <th>Invoice Date</th>      <th>Invoice Number</th>      <th>Doc Total</th>      <th>Paid To Invoice</th>      <th>Payment Number</th>      <th>Payment Date</th>      <th>Remaining Payment</th>    </tr>    <!-- Fin de encabezado -->    <!-- Inicio de Columnas para datos -->    " + rows + "  </table>  <!-- Fin de Columnas para datos -->  <div style='text-align: right; display: flex; flex-direction: row; justify-content: flex-end;'>    <h3>      Total Remaining Payment:  $ " + totalRemainingPayment + " </h3>    <p align=right>          </p>  </div></div>";

            return html;
        }
    }
}