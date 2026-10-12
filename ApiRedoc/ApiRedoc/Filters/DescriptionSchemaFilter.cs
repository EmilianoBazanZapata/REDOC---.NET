using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiRedoc.Filters
{
    /// <summary>
    /// Copies the text of each property's <see cref="DescriptionAttribute"/> into the generated schema,
    /// so models can be documented without XML comments.
    /// </summary>
    public class DescriptionSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null || schema.Properties.Count == 0) return;

            foreach (var property in context.Type.GetProperties())
            {
                var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();

                if (descriptionAttribute == null) continue;

                // Schema property names follow the JSON naming policy (camelCase by default).
                var schemaName = JsonNamingPolicy.CamelCase.ConvertName(property.Name);

                if (schema.Properties.TryGetValue(schemaName, out var propertySchema))
                    propertySchema.Description = descriptionAttribute.Description;
            }
        }
    }
}
