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
                await WriteError(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (RpcException)
            {
                await WriteError(
                    context,
                    "Gateway is not reachable",
                    HttpStatusCode.BadGateway);
            }
            catch (TimeoutException)
            {
                await WriteError(
                    context,
                    "Operation timeout",
                    HttpStatusCode.RequestTimeout);
            }
            catch (Exception)
            {
                await WriteError(
                    context,
                    "Unexpected server error",
                    HttpStatusCode.InternalServerError);
            }
        }

        private static async Task WriteError(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>(
                data: Array.Empty<object>(),
                success: false,
                message: message
            );

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
