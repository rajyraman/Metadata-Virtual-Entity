using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RYR.VE.DataProviders.Shared.Extensions;

namespace RYR.VE.DataProviders.Shared.Helpers
{
    internal class QueryVisitorHelpers
    {
        public static void ApplyConditions(List<Entity> entities, CustomQueryVisitor visitor)
        {
            foreach (var condition in visitor.Conditions)
            {
                int.TryParse(condition.Values[0]?.ToString(), out var conditionNumber);
                switch (condition.Operator)
                {
                    case ConditionOperator.Equal:
                        entities.RemoveAll(x => 
                            !x.GetAttributeValue(condition.AttributeName).Equals(condition.Values[0]));
                        break;
                    case ConditionOperator.NotEqual:
                        entities.RemoveAll(x => 
                            x.GetAttributeValue(condition.AttributeName).Equals(condition.Values[0]));
                        break;
                    case ConditionOperator.LessThan:
                        entities.RemoveAll(x => 
                            (int)x.GetAttributeValue(condition.AttributeName) >= conditionNumber);
                        break;
                    case ConditionOperator.GreaterThan:
                        entities.RemoveAll(x => 
                            (int)x.GetAttributeValue(condition.AttributeName) <= conditionNumber);
                        break;
                    case ConditionOperator.LessEqual:
                        entities.RemoveAll(x => 
                            (int)x.GetAttributeValue(condition.AttributeName) > conditionNumber);
                        break;
                    case ConditionOperator.GreaterEqual:
                        entities.RemoveAll(x => 
                            (int)x.GetAttributeValue(condition.AttributeName) < conditionNumber);
                        break;
                    case ConditionOperator.Like:
                        entities.RemoveAll(x =>
                        {
                            var entityStringValue = x.GetAttributeValue(condition.AttributeName).ToString();
                            var conditionStringValue = condition.Values[0].ToString();
                            conditionStringValue = conditionStringValue.Substring(1, conditionStringValue.Length - 2);
                            return !entityStringValue.Contains(conditionStringValue);
                        });
                        break;
                    case ConditionOperator.NotLike:
                        entities.RemoveAll(x =>
                        {
                            var entityStringValue = x.GetAttributeValue(condition.AttributeName).ToString();
                            var conditionStringValue = condition.Values[0].ToString();
                            conditionStringValue = conditionStringValue.Substring(1, conditionStringValue.Length - 2);
                            return entityStringValue.Contains(conditionStringValue);
                        });
                        break;
                }
            }
        }
        public static EntityCollection SetPaging(List<Entity> results, CustomQueryVisitor visitor)
        {
            var totalCount = results.Count;
            var resultEntityCollection = new EntityCollection
            {
                TotalRecordCount = totalCount,
                MoreRecords = (totalCount / visitor.Count) > visitor.PageNumber
            };
            return resultEntityCollection;
        }
        public static List<Entity> OrderResults(CustomQueryVisitor visitor, List<Entity> entities)
        {
            if (!visitor.Orders.Any()) return entities;

            var firstOrderBy = visitor.Orders.First();
            var orderedFilteredEntities = SortResults(firstOrderBy, entities);
            foreach (var orderBy in visitor.Orders.Skip(1).ToList())
            {
                orderedFilteredEntities = SortResults(orderBy, orderedFilteredEntities, true);
            }

            return orderedFilteredEntities.ToList();
        }

        private static IOrderedEnumerable<Entity> SortResults(OrderExpression orderBy, IEnumerable<Entity> entities, bool isContinue = false)
        {
            if (isContinue)
            {
                return orderBy.OrderType == OrderType.Ascending
                    ? ((IOrderedEnumerable<Entity>)entities)
                    .ThenBy(x => x.GetAttributeValue(orderBy.AttributeName))
                    : ((IOrderedEnumerable<Entity>)entities)
                    .ThenByDescending(x => x.GetAttributeValue(orderBy.AttributeName));
            }

            return orderBy.OrderType == OrderType.Ascending
                ? entities
                    .OrderBy(x => x.GetAttributeValue(orderBy.AttributeName))
                : entities
                    .OrderByDescending(x => x.GetAttributeValue(orderBy.AttributeName));
        }
    }
}
