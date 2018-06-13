using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace RYR.VE.DataProviders.Shared
{
    internal class CustomQueryVisitor : IQueryExpressionVisitor
    {
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public List<OrderExpression> Orders { get; set; }
        public List<ConditionExpression> Conditions { get; set; }

        public QueryExpression Visit(QueryExpression query)
        {
            Count = query.PageInfo.Count;
            PageNumber = query.PageInfo.PageNumber;
            Orders = query.Orders.ToList();
            Conditions = query.Criteria.Conditions.ToList();
            return query;
        }
    }
}