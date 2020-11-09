using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Test.Contexts;
using Test.Entities;
using Test.Models;
using Test.Services;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IEnumerable<ICurrency> currencys;
        private readonly ILogger<PurchasesController> logger;

        public PurchasesController(ApplicationDbContext context, IMapper mapper, IEnumerable<ICurrency> currencys, ILogger<PurchasesController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.currencys = currencys;
            this.logger = logger;
        }

        [HttpGet("{id}", Name = "GetPurchase")]
        public async Task<ActionResult<PurchaseDTO>> Get(int id)
        {
            var purchase = await context.Purchases.FindAsync(id);

            if (purchase == null)
                return NotFound();

            var purchaseDTO = mapper.Map<PurchaseDTO>(purchase);
            return purchaseDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PurchaseCreationDTO purchaseCreation)
        {
         
            var purchases = await context.Purchases.Where(x =>
                x.UserId == purchaseCreation.UserId &&
                x.Date.Month == DateTime.Today.Month && 
                x.IsoCurrency == purchaseCreation.IsoCurrency.ToUpper())
                .ToListAsync();

            var selectedCurrency = currencys.SingleOrDefault(x => x.CurrencyIso == purchaseCreation.IsoCurrency.ToUpper());
            var purchaseData = await selectedCurrency.GetPurchaseData(purchases, purchaseCreation.Amount);

            if (!purchaseData.EnabledPurchase)
            {
                logger.LogWarning($"purchase denaid, User ID {purchaseCreation.UserId}");
                return BadRequest();
            }

            var purchase = mapper.Map<Purchase>(purchaseCreation);
            purchase.Date = DateTime.Now;
            purchase.AmountCurrencyPurchased = purchaseData.AmountCurrencyPurchased;
            purchase.ExchangeRate = purchaseData.ExchangeRate;
            purchase.SellRate = purchaseData.Sell;
            purchase.BuyRate = purchaseData.Buy;

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();

            var purchaseDTO = mapper.Map<PurchaseDTO>(purchase);
            purchaseDTO.Date = purchase.Date;
            purchaseDTO.AmountCurrencyPurchased = purchase.AmountCurrencyPurchased;

            logger.LogInformation($"Purchase Succes, User ID {purchaseCreation.UserId}");
            return new CreatedAtRouteResult("GetPurchase", new { id = purchase.Id }, purchaseDTO);
        }
    }
}
