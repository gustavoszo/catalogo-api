using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatalogoApi.Exceptions
{

    public class ExceptionHandlerMiddleware
    {
        // O RequestDelegate é uma referência para o próximo middleware no pipeline. O next permite que a execução continue após este middleware.
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Aqui, o método intercepta o processamento da requisição:
        // Ele tenta executar o próximo middleware.
        // Se uma exceção for lançada, ela será capturada no bloco catch e passada para o método HandleExceptionAsync.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                EntityNotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorMessage = new ErrorMessage(
                statusCode,
                exception.Message,
                context.Request.Path
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(errorMessage.ToJson());
        }
    }

}
