using JWT.Exceptions;
using Kernel.Domain.Model.Helpers;
using Kernel.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Univem.Churras.Api.Attributes
{
    public class ChurrasAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly ITokenHelper _tokenHelper;

        public ChurrasAuthorizeAttribute()
        {
            _tokenHelper = IoC.Get<ITokenHelper>();
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var tokenString = context.HttpContext.Request.Headers["Token"].ToString();
                if(!string.IsNullOrWhiteSpace(tokenString))
                {
                    var token = _tokenHelper.GetToken(tokenString);
                    if (token == null || token.ExpiresIn < DateTime.Now)
                        context.Result = new UnauthorizedResult();
                }
                else
                    context.Result = new UnauthorizedResult();
            }
            catch(SignatureVerificationException)
            {
                context.Result = new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                context.Result = new StatusCodeResult(500);
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
