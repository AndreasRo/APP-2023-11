using ConsumerBank.Services;
using ConsumerBank.Services.Contracts;
using ConsumerBank.Services.Options;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerBankApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILogger<LoansController> _logger;
        private readonly ILoanerService _loanerService;

        public LoansController(ILogger<LoansController> logger, ILoanerService loanerService)
        {
            _logger = logger;
            // var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            // _dbOptions = new DbOptions
            // {
            //     Keyvault = configuration.GetValue<string>("KeyvaultUri"),
            //     Database = configuration.GetValue<string>("LoansDbUri"),
            // };
        }

        [HttpPost]
        [Route("Apply")]
        public IActionResult ApplyForLoan([FromBody]LoanRequest request)
        {
            _logger.Log(LogLevel.Information, "Got application");
            var approved = _loanerService.Apply(request);
            return Ok(approved);
        }

        [HttpGet]
        public IActionResult Ping()
        {
            _logger.Log(LogLevel.Information, "Ping");
            return Ok("Hello world");
        }
    }
}