using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using RYR.VE.DataProviders.Shared;
using RYR.VE.DataProviders.Shared.Extensions;

namespace RYR.VE.DataProviders.AttributeDataProvider
{
    public class AttributeRetrieveMultiple : RetrieveMultiplePluginBase
    {
        public override List<Entity> RetrieveRecords(IOrganizationService service) => service.RetrieveAttributes();
    }
}