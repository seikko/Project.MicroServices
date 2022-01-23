using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.FakePayment.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomeBasController
    {

        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}
