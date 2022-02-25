using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SAPB1WordPressAPI.DataModel.DAL;
using SAPB1WordPressAPI.DataModel.Entities;
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

        [HttpGet("[action]/cardCode")]
        public async Task<IActionResult> GenerateCustomerStatement(string cardCode)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync($"<div><h1>Hello PDF world!</h1><h2 style='color: red; text-align: center;'>Greeting <i>HTML</i> world</h2></div>");
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });
            return File(pdfContent, "application/pdf", "converted.pdf");
        }
    }
}