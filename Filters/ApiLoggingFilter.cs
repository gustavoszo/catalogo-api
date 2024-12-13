using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogoApi.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {

        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executando antes da Action");
            _logger.LogInformation("############################################");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executando depois da Action");
            _logger.LogInformation($"Status code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("############################################");
        }
    }
}
