using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChaosController : ControllerBase
    {
        [HttpGet]
        [Route("switch")]
        public async Task<bool> Swich()
        {
            Startup.ChaosEnabled = !Startup.ChaosEnabled;
            return Startup.ChaosEnabled;
        }
    }
}
