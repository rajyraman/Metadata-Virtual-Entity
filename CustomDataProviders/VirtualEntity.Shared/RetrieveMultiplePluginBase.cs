using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Query;
using RYR.VE.DataProviders.Shared.Definitions;
using RYR.VE.DataProviders.Shared.Helpers;

namespace RYR.VE.DataProviders.Shared
{
    public abstract class RetrieveMultiplePluginBase : PluginBase
    {
        public override void Execute(ITracingService tracer, IOrganizationService service, IPluginExecutionContext context)
        {
            var query = context.InputParameterOrDefault<QueryExpression>(ContextProperties.QueryExpression);
            if (query == null) return;
            tracer.Trace($"RetrieveMultiple ${query.EntityName}");
            var visitor = new CustomQueryVisitor();
            query.Accept(visitor);
            var records = RetrieveRecords(service);
            PrepareResults(visitor, records, context);
            tracer.Trace($"Set output with ${records.Count} records");
        }
        private void PrepareResults(CustomQueryVisitor visitor, List<Entity> entities, IPluginExecutionContext context)
        {
            var results = QueryVisitorHelpers.OrderResults(visitor, entities);
            QueryVisitorHelpers.ApplyConditions(results, visitor);
            var resultEntityCollection = QueryVisitorHelpers.SetPaging(results, visitor);
            ObjectHelper.SetOutput(resultEntityCollection, results, visitor, context);
        }

        public abstract List<Entity> RetrieveRecords(IOrganizationService service);
    }
}