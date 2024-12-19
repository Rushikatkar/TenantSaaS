using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation_Layer.Middlewares
{
    public class OptionalAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAllowAnonymous = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
                .Any();

            if (hasAllowAnonymous)
            {
                operation.Security?.Clear();
            }
        }
    }
}
