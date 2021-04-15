using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.Authentication.Commands.SignIn;
using Mediaverse.Application.Authentication.Commands.SignUp;
using Mediaverse.Application.Authentication.Commands.SignUpAnonymous;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mediaverse.Web.Models;

namespace Mediaverse.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        
        private readonly ILogger<HomeController> _logger;

        public AuthenticationController(
            IMediator mediator,
            ILogger<HomeController> logger)
        {
            _mediator = mediator;
            
            _logger = logger;
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInCommand signIn)
        {
            var user = await _mediator.Send(signIn);
            
            return Json(new
            {
                redirectToUrl = @Url.Action(
                    "Index",
                    "ContentConsumption",
                    new { viewerId = user.Id})
            });
        }
        
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpCommand command)
        {
            var user = await _mediator.Send(command);
            
            return Json(new
            {
                redirectToUrl = @Url.Action(
                    "Index",
                    "ContentConsumption",
                    new { viewerId = user.Id})
            });
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAnonymous()
        {
            var command = new SignUpAnonymousCommand();
            var user = await _mediator.Send(command);

            return Json(new
            {
                redirectToUrl = @Url.Action(
                    "Index",
                    "ContentConsumption",
                    new { viewerId = user.Id})
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}