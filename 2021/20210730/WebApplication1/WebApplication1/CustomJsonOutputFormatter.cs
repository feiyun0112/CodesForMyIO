using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class CustomJsonOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter
    {
        private readonly SystemTextJsonOutputFormatter pascalCaseFormater;
        public CustomJsonOutputFormatter(JsonSerializerOptions jsonSerializerOptions) : base(jsonSerializerOptions)
        {
            var newOptions = new JsonSerializerOptions(jsonSerializerOptions);
            newOptions.PropertyNamingPolicy = null;
            pascalCaseFormater = new SystemTextJsonOutputFormatter(newOptions);
        }

        public override Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (GetFormat(context) == "json2")
            {
                return pascalCaseFormater.WriteAsync(context);
            }
            return base.WriteAsync(context);
        }

        private string? GetFormat(OutputFormatterWriteContext context)
        {
            if (context.HttpContext.Request.RouteValues.TryGetValue("format", out var obj))
            {
                var routeValue = Convert.ToString(obj, CultureInfo.InvariantCulture);
                return string.IsNullOrEmpty(routeValue) ? null : routeValue;
            }

            var query = context.HttpContext.Request.Query["format"];
            if (query.Count > 0)
            {
                return query.ToString();
            }

            return "json";
        }
    }
}
