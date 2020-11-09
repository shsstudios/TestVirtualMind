using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IEnumerable<ICurrency> currencys;
        private readonly ILogger<RateController> logger;

        public RateController(IEnumerable<ICurrency> currencys, ILogger <RateController> logger)
        {
            this.currencys = currencys;
            this.logger = logger;
        }

        [HttpGet("{currency}")]
        public async Task<ActionResult<RateData>> GetAsync(string currency)
        {
            logger.LogInformation("Get Exchange rate");
           var selectedCurrency = currencys.SingleOrDefault(x => x.CurrencyIso == currency.ToUpper());

            if (selectedCurrency == null)
            {
                logger.LogWarning($"Exchange currency not supported: {currency.ToUpper()}");
                return NotFound();
            }

            return await selectedCurrency.GetRate();
        }
    }
}
