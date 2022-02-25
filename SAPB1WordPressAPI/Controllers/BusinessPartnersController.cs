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

        // GET: api/GetBusinessPartnersCreditLimit/
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<OCRD>>> GetBusinessPartnersCreditLimit()
        {
            return await sapDbContext.OCRD.AsNoTracking().Take(10).ToListAsync();
        }

        [HttpGet("[action]/cardCode")]
        public ActionResult<OCRD> GetBusinessPartnersCreditLimitByCardCode(string cardCode)
        {
            return sapDbContext.OCRD.AsNoTracking().Where(x => x.CardCode == cardCode).FirstOrDefault();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GenerateCustomerStatement(/*string cardCode , DateTime startDate, DateTime endDate*/)
        {
            var cardCode = "129564";
            var startDate = new DateTime(2018, 01, 01);
            var endDate = new DateTime(2022, 12, 31);

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
            string cardName="";

            if(details != null)
            {
                cardName = details.First().CustomerName;
            }

            foreach (var dt in details)
            {

                rows += $"<tr>      <td> {dt.InvoicePostingDate.ToShortDateString()} </td>      <td> {dt.InvDocumentNumber} </td>      <td> {dt.DocTotal} </td>      <td> {dt.PaidToInvoice} </td>      <td> {dt.PaymentDocumentNumber}</td>      <td> {dt.PaymentPostingDate.ToShortDateString()} </td>      <td> {dt.RemainingPayment} </td>    </tr>";
            }

            string html = "<div>  <h1>Customer Statement</h1>  <h3>"+cardCode+" - "+cardName+"</h3>  <style>    th {      border-bottom: 2px solid black;    }  </style>  <table style='width: 100%'>    <!-- Inicio de encabezado -->    <tr>      <th>Invoice Posting Date</th>      <th>Inv Document Number</th>      <th>Doc Total</th>      <th>Paid To Invoice</th>      <th>Payment Document Number</th>      <th>Payment Posting Date</th>      <th>Remaining Payment</th>    </tr>    <!-- Fin de encabezado -->    <!-- Inicio de Columnas para datos -->    " + rows + "  </table></div>";

            return html;
        }
    }
}