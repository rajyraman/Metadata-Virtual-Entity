using System;
using Microsoft.Xrm.Sdk;
using RYR.VE.DataProviders.Shared;
using RYR.VE.DataProviders.Shared.Extensions;

namespace RYR.VE.DataProviders.EntityDataProvider
{
    public class EntityRetrieve : RetrievePluginBase
    {
        public override Entity RetrieveRecord(IOrganizationService service, Guid id) => service.RetrieveEntity(id);
    }
}