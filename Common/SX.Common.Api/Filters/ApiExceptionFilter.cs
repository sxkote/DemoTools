using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SX.Common.Shared.Exceptions;

namespace SX.Common.Api.Filters
{
    public class ApiExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is CustomException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = exception.StatusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
