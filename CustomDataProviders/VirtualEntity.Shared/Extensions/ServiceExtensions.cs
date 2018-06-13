using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using RYR.VE.DataProviders.Shared.Helpers;

namespace RYR.VE.DataProviders.Shared.Extensions
{
    internal static class ServiceExtensions
    {
        private static EntityMetadata RetrieveEntity(this IOrganizationService service, string logicalName)
        {
            var retrieveEntityRequest = new RetrieveEntityRequest
            {
                LogicalName = logicalName,
                RetrieveAsIfPublished = true
            };
            var retrieveEntityResponse = ((RetrieveEntityResponse)service.Execute(retrieveEntityRequest)).EntityMetadata;
            return retrieveEntityResponse;
        }
        private static EntityMetadata[] RetrieveEntities(this IOrganizationService service, EntityFilters type)
        {
            var retrieveEntityRequest = new RetrieveAllEntitiesRequest
            {
                EntityFilters = type,
                RetrieveAsIfPublished = true
            };
            var retrieveEntityResponse = ((RetrieveAllEntitiesResponse) service.Execute(retrieveEntityRequest)).EntityMetadata;
            return retrieveEntityResponse;
        }
        public static Entity RetrieveEntity(this IOrganizationService service, Guid metadataId)
        {
            var retrieveEntityResponse = service.RetrieveEntities(EntityFilters.Entity);
            return ObjectHelper.CreateEntity(retrieveEntityResponse.Single(x=>x.MetadataId == metadataId));
        }
        public static List<Entity> RetrieveEntities(this IOrganizationService service)
        {
            var retrieveEntityResponse = service.RetrieveEntities(EntityFilters.Entity);
            return ObjectHelper.CreateEntity(retrieveEntityResponse);
        }

        public static List<Entity> RetrieveAttributes(this IOrganizationService service)
        {
            var retrieveEntityResponse = service.RetrieveEntities(EntityFilters.Attributes);
            return ObjectHelper.CreateAttribute(retrieveEntityResponse);
        }
        public static Entity RetrieveAttribute(this IOrganizationService service, Guid metadataId)
        {
            var attributeMetadata = ((RetrieveAttributeResponse)service.Execute(new RetrieveAttributeRequest
            {
                MetadataId = metadataId,
                RetrieveAsIfPublished = true
            })).AttributeMetadata;
            var entityMetadata = service.RetrieveEntity(attributeMetadata.EntityLogicalName);
            return ObjectHelper.CreateAttribute(attributeMetadata, entityMetadata);
        }
    }
}
