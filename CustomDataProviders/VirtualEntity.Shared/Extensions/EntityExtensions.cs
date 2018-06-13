using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace RYR.VE.DataProviders.Shared.Extensions
{
    internal static class EntityExtensions
    {
        public static object GetAttributeValue(this Entity entity, string attributeName)
        {
            entity.Attributes.TryGetValue(attributeName, out var attributeValue);
            if (attributeValue == null) return string.Empty;

            switch (attributeValue)
            {
                case OptionSetValue o:
                    return o.Value;
                case EntityReference e:
                    return e.Id;
                case bool o:
                    return o;
                case int i:
                    return i;
                default:
                    return attributeValue?.ToString();
            }
        }
    }
}
