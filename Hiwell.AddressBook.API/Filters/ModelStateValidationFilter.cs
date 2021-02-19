using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.API.Filters
{
    public class ModelStateValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                var json = new
                {
                    ValidationErrors = validationErrors.Select(x => new { fieldName = x.Key, messages = x.Value })
                };

                context.Result = new BadRequestObjectResult(json);

                return;
            }

            await next();
        }
    }
}
