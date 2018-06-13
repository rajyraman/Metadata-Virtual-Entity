using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using RYR.VE.DataProviders.Shared.Definitions;

namespace RYR.VE.DataProviders.Shared.Helpers
{
    internal class ObjectHelper
    {
        public static void SetOutput(EntityCollection results, IEnumerable<Entity> resultEntityCollection, CustomQueryVisitor visitor,
            IPluginExecutionContext context)
        {
            results.Entities.AddRange(resultEntityCollection
                .Skip(visitor.PageNumber == 1 ? 0 : visitor.PageNumber * visitor.Count)
                .Take(visitor.Count).ToList());
            context.OutputParameters[ContextProperties.Results] = results;
        }
        public static Entity CreateEntity(EntityMetadata e)
        {
            return
                new Entity
                {
                    Id = e.MetadataId.GetValueOrDefault(),
                    LogicalName = ryr_entity.EntityName,
                    Attributes =
                    {
                        [ryr_entity.PrimaryKey] = e.MetadataId.GetValueOrDefault(),
                        [ryr_entity.PrimaryName] = e.LogicalName,
                        [ryr_entity.ryr_logicalname] = e.LogicalName,
                        [ryr_entity.ryr_displayname] = e.DisplayName?.UserLocalizedLabel?.Label,
                        [ryr_entity.ryr_objecttypecode] = e.ObjectTypeCode.GetValueOrDefault(),
                        [ryr_entity.ryr_ownershiptype] =
                            e.OwnershipType.HasValue ? new OptionSetValue((int) e.OwnershipType) : null,
                        [ryr_entity.ryr_entitytype] = new OptionSetValue(e.DataProviderId.HasValue ? 2 : 1),
                        [ryr_entity.ryr_displaycollectionname] = e.DisplayCollectionName?.UserLocalizedLabel?.Label,
                        [ryr_entity.ryr_entitysetname] = e.EntitySetName,
                        [ryr_entity.ryr_dayssincerecordlastmodified] =
                            e.DaysSinceRecordLastModified.GetValueOrDefault(),
                        [ryr_entity.ryr_isactivity] = e.IsActivity.GetValueOrDefault(),
                        [ryr_entity.ryr_iscustomentity] = e.IsCustomEntity.GetValueOrDefault(),
                        [ryr_entity.ryr_iscustomizable] = e.IsCustomizable?.Value,
                        [ryr_entity.ryr_isintersect] = e.IsIntersect.GetValueOrDefault(),
                        [ryr_entity.ryr_isquickcreateenabled] = e.IsQuickCreateEnabled.GetValueOrDefault(),
                        [ryr_entity.ryr_isrenameable] = e.IsRenameable?.Value,
                        [ryr_entity.ryr_isvalidforadvancedfind] = e.IsValidForAdvancedFind.GetValueOrDefault(),
                        [ryr_entity.ryr_ischangedtrackingenabled] = e.ChangeTrackingEnabled.GetValueOrDefault(),
                        [ryr_entity.ryr_primaryidattribute] = e.PrimaryIdAttribute,
                        [ryr_entity.ryr_primarynameattribute] = e.PrimaryNameAttribute
                    }
                };
        }

        internal static Entity CreateAttribute(AttributeMetadata a, EntityMetadata e)
        {
            return new Entity
            {
                Id = a.MetadataId.GetValueOrDefault(),
                LogicalName = ryr_attribute.EntityName,
                Attributes =
                {
                    [ryr_attribute.PrimaryKey] = a.MetadataId.GetValueOrDefault(),
                    [ryr_attribute.PrimaryName] = a.LogicalName,
                    [ryr_attribute.ryr_logicalname] = a.LogicalName,
                    [ryr_attribute.ryr_attributetype] = new OptionSetValue((int) a.AttributeType),
                    [ryr_attribute.ryr_description] = a.Description?.UserLocalizedLabel?.Label,
                    [ryr_attribute.ryr_displayname] = a.DisplayName?.UserLocalizedLabel?.Label,
                    [ryr_attribute.ryr_iscustomattribute] = a.IsCustomAttribute,
                    [ryr_attribute.ryr_requiredlevel] = new OptionSetValue((int) a.RequiredLevel.Value),
                    [ryr_attribute.ryr_entityid] = new EntityReference(ryr_entity.EntityName,
                        e.MetadataId.Value)
                    {
                        Name = e.DisplayName?.UserLocalizedLabel?.Label ?? e.LogicalName
                    },
                }
            };
        }

        public static List<Entity> CreateEntity(EntityMetadata[] retrieveEntityResponse)
        {
            var entities = (from e in retrieveEntityResponse
                where e.IsPrivate == false
                orderby e.LogicalName
                select CreateEntity(e)).ToList();
            return entities;
        }

        public static List<Entity> CreateAttribute(EntityMetadata[] retrieveEntityResponse)
        {
            var attributes = new List<Entity>();

            foreach (var entityMetadata in retrieveEntityResponse)
            {
                var entityAttributes = from a in entityMetadata.Attributes
                    where a.IsLogical == false
                    orderby a.LogicalName
                    select CreateAttribute(a, entityMetadata);
                attributes.AddRange(entityAttributes.ToList());
            }

            return attributes;
        }
    }
}
