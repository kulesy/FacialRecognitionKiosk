using DsReceptionClassLibrary.Domain.Entities.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DsReceptionAPI.Application.PipelineBehaviours
{
    public class ResponseMappingFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Result is ObjectResult objectResult && objectResult.Value is CQRSResponse cqrsResponse)
            {
                if (cqrsResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    context.Result = new ObjectResult(new { cqrsResponse.Messages }) { StatusCode = (int)cqrsResponse.StatusCode };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
