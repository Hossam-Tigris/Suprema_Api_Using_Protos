using Grpc.Core;
using System.Net;
using System.Text.Json;

namespace Suprema_Api_Using_Protos.Helper
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (RpcException ex)
            {
                await WriteError(context, HttpStatusCode.BadGateway,
                    "Device/Gateway error: " + ex.Status.Detail);
            }
            catch (TimeoutException ex)
            {
                await WriteError(context, HttpStatusCode.RequestTimeout, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteError(context, HttpStatusCode.InternalServerError,
                    "Unexpected error", ex);
            }
        }

        private static async Task WriteError(
            HttpContext context,
            HttpStatusCode code,
            string message,
            Exception? ex = null)
        {
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            var result = new
            {
                success = false,
                message = message,
                data = Array.Empty<object>(),
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
