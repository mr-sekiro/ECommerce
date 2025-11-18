using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace ECommerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationError(ActionContext context)
        {
            var errors = context.ModelState
                        .Where(kvp => kvp.Value?.Errors != null && kvp.Value.Errors.Count > 0)
                        .Select(kvp => new Shared.ErrorModels.ValidationError
                        {
                            Field = kvp.Key,
                            Errors = kvp.Value!.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message ?? "Invalid value" : e.ErrorMessage)
                        })
                        .ToList();

            var response = new ValidationErrorToReturn
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Validation Failed",
                ValidationErrors = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
