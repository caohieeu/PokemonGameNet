using Newtonsoft.Json;
using PokemonGame.Exceptions;
using PokemonGame.Models.Response;
using System.Net;

namespace PokemonGame.Middlewares
{
    public class ResponseApiMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            ApiResponse response;

            switch(ex)
            {
                case NotFoundException _:
                    response = new ApiResponse()
                    {
                        code = (int)HttpStatusCode.NotFound,
                        message = ex.Message,
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException _:
                    response = new ApiResponse()
                    {
                        code = (int)HttpStatusCode.BadRequest,
                        message = ex.Message,
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case AuthorizationException _:
                    response = new ApiResponse()
                    {
                        code = (int)HttpStatusCode.Unauthorized,
                        message = ex.Message,
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response = new ApiResponse
                    {
                        code = 500,
                        message = ex.Message,
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
