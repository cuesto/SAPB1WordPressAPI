using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}