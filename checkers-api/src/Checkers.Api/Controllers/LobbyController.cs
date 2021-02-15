using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkers.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Checkers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly ILogger<LobbyController> _logger;
        private readonly IHubContext<CheckersHub> _checkersHubContext;

        public LobbyController(ILogger<LobbyController> logger, IHubContext<CheckersHub> hubContext)
        {
            _logger = logger;
            _checkersHubContext = hubContext;
        }

    }
}
