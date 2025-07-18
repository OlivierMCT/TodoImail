using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoImail.Services.Models;

namespace TodoImail.WebApi.Filters {
    public class TodoImailExceptionFilterAttribute : ExceptionFilterAttribute {
        public override void OnException(ExceptionContext context) {
            if(context.Exception is TodoImailServiceException ex) {
                context.Result = ex.Code switch {
                    1 or 2 => new NotFoundObjectResult(ex.Message),
                    4 => new BadRequestObjectResult(ex.Message.Split("|")
                        .Select(err => new { Property = err.Split(":").First(), Message = err.Split(":").Last() })
                        .ToDictionary(err => err.Property, err => err.Message)),
                    3 => new BadRequestObjectResult(ex.Message),
                    _ => throw ex
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
