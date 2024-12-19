using Serilog.Context;

namespace Presentation_Layer.Middlewares
{
    public class TenantLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tenantId))
            {
                // Add tenant information to the log context
                LogContext.PushProperty("TenantId", tenantId);
            }

            await _next(context);
        }
    }

}
