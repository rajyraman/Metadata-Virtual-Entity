using System;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm.Sdk;

namespace RYR.VE.DataProviders.Shared
{
    public abstract class PluginBase : IPlugin
    {
        public abstract void Execute(ITracingService tracer, IOrganizationService service, IPluginExecutionContext context);

        public void Execute(IServiceProvider serviceProvider)
        {
            var tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = factory.CreateOrganizationService(context.UserId);

            try
            {
                Execute(tracer, service, context);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }

    }
}