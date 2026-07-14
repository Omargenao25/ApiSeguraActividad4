using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ApiSeguraActividad4.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-API-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
        
            var extractedApiKey = configuration.GetValue<string>("ApiSettings:ApiKey");

          
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedHeaderApiKey)
                || extractedHeaderApiKey != extractedApiKey)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Acceso denegado. API Key inválida o no proporcionada.");
                return;
            }

           
            await _next(context);
        }
    }
}