using System;
using Microsoft.Xrm.Sdk;
using RYR.VE.DataProviders.Shared;
using RYR.VE.DataProviders.Shared.Extensions;

namespace RYR.VE.DataProviders.AttributeDataProvider
{
    public class AttributeRetrieve : RetrievePluginBase
    {
        public override Entity RetrieveRecord(IOrganizationService service, Guid id) => service.RetrieveAttribute(id);
    }
}