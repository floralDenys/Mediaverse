using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.Authentication.Commands.SignIn;
using Mediaverse.Application.Authentication.Commands.SignOut;
using Mediaverse.Application.Authentication.Commands.SignUp;
using Mediaverse.Application.Authentication.Commands.SignUpAnonymous;
using Microsoft.AspNetCore.Mvc;
using Mediaverse.Web.Models;

namespace Mediaverse.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInCommand signIn)
        {
            try
            {
                await _mediator.Send(signIn);
                return Json(new
                {
                    redirectToUrl = @Url.Action("Index", "ContentConsumption")
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message});
            }
        }
        
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Json(new
                {
                    redirectToUrl = @Url.Action("Index", "ContentConsumption")
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message});
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignUpAnonymous(string redirectUrl)
        {
            try
            {
                var command = new SignUpAnonymousCommand();
                await _mediator.Send(command);

                return Json(new
                {
                    redirectToUrl = redirectUrl
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message});
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignOut(CancellationToken cancellationToken)
        {
            var command = new SignOutCommand();
            await _mediator.Send(command, cancellationToken);
            
            return RedirectToAction("Index", "Home");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}