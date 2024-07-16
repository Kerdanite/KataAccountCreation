using Microsoft.AspNetCore.Mvc;

namespace AccountManagementApi.Accounts
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateAccount(CreateAccount account)
        {
            return Created(string.Empty, null);
        }
    }
}
