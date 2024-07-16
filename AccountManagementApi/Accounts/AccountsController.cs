using AccountManagement.Application.CreateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementApi.Accounts
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly ISender _sender;

        public AccountsController(ILogger<AccountsController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccount account)
        {
            var command = new CreateAccountCommand(account.UserName);

            var result = await _sender.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Created(string.Empty, null);
        }
    }
}
