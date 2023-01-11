using GadgetsAPI.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace GadgetsAPI.Web
{
    public class AppExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is GadgetNotFoundException gnfe)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "gadget-not-found",
                        gadgetId = gnfe.GadgetId
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }
                else if (context.Exception is CategoryNotFoundException cnfe)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "category-not-found",
                        categoryId = cnfe.CategoryId
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }
                else if (context.Exception is ProducerNotFoundException pnfe)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "producer-not-found",
                        categoryId = pnfe.ProducerId
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }


                else if (context.Exception is BadRequestException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "bad-request"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }


                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
