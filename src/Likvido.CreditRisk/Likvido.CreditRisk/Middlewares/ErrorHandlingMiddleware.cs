using Likvido.CreditRisk.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private static readonly Dictionary<Type, HttpStatusCode> ExceptionStatusCodes = new Dictionary<Type, HttpStatusCode>
        {
            [typeof(ResourceNotFoundException)] = HttpStatusCode.NotFound
        };


        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string errorMessage = string.Empty;

            var exceptionType = exception.GetType();
            if (ExceptionStatusCodes.ContainsKey(exceptionType))
            {
                code = ExceptionStatusCodes[exceptionType];
                errorMessage = exception.Message;
            }

            // TODO log error

            var result = JsonConvert.SerializeObject(new { error = errorMessage });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
