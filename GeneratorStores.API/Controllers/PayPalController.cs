using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GeneratorStores.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayPalController : ControllerBase
    {
        private readonly IOptions<PayPalSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public PayPalController(IOptions<PayPalSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] string transactionId)
        {
            // Use _settings.Value.ClientId and _settings.Value.Secret
            // Make server-side API call to PayPal to verify
            return Ok();
        }
    }

    public class PayPalSettings
    {
        public string ClientId { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }

}
