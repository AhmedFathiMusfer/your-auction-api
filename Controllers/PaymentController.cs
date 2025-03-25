using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace your_auction_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        public PaymentController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("PaymentController");
        }
    }
}