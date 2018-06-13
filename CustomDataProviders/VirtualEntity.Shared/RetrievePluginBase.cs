using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using RYR.VE.DataProviders.Shared.Definitions;

namespace RYR.VE.DataProviders.Shared
{
    public abstract class RetrievePluginBase : PluginBase
    {
        public override void Execute(ITracingService tracer, IOrganizationService service, IPluginExecutionContext context)
        {
            var target = context.InputParameterOrDefault<EntityReference>(ContextProperties.Target);
            if (target == null) return;
            tracer.Trace($"Retrieve ${target.LogicalName} with id {target.Id}");

            var targetEntity = RetrieveRecord(service, target.Id);
            context.OutputParameters[ContextProperties.Result] = targetEntity;
            tracer.Trace($"Set ${ContextProperties.Result} with {targetEntity.LogicalName}({targetEntity.Id})");
        }
        public abstract Entity RetrieveRecord(IOrganizationService service, Guid id);
    }
}