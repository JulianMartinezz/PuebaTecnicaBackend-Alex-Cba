using HR_Medical_Records.DTOs.SortingDTOs;
using System.Linq.Expressions;
using System.Reflection;

namespace HR_Medical_Records.Helpers
{
    public static class SortingHelper
    {
        public static IQueryable<T> ApplySortingAndPagination<T>(IQueryable<T> query, Basefilter filter, bool applyPagination = false)
        {
            if (!string.IsNullOrEmpty(filter.ColumnFilter))
            {
                string methodName = filter.SortBy == SORTBY.DESC ? "OrderByDescending" : "OrderBy";
                var type = typeof(T);
                var property = FindProperty(type, filter.ColumnFilter, out List<PropertyInfo> parentProperty);

                if (property != null)
                {
                    var parameter = Expression.Parameter(type, "p");
                    var propertyAccess = parentProperty == null ? Expression.MakeMemberAccess(parameter, property) : CreatePropertyAccess(parameter, parentProperty, property);
                    var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                    var resultExpression = Expression.Call(typeof(Queryable), methodName,
                        new Type[] { type, property.PropertyType },
                        query.Expression, Expression.Quote(orderByExpression));

                    query = query.Provider.CreateQuery<T>(resultExpression);
                }
            }

            if (!applyPagination)
            {
                return query;
            }

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Limit.HasValue)
            {
                query = query.Take(filter.Limit.Value);
            }

            return query;
        }

        private static PropertyInfo FindProperty(Type type, string propertyName, out List<PropertyInfo> parentProperty, int flag = 1)
        {
            var properties = type.GetProperties();

            if (flag >= 3)
            {
                parentProperty = null;
                return null;
            }
            foreach (var prop in properties)
            {
                if (prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    parentProperty = null;
                    return prop;
                }

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    var innerProperty = FindProperty(prop.PropertyType, propertyName, out parentProperty, flag + 1);
                    if (innerProperty != null)
                    {
                        if (parentProperty == null)
                        {
                            parentProperty = new List<PropertyInfo>() { prop };
                        }
                        else
                        {
                            parentProperty.Add(prop);
                        }
                        return innerProperty;
                    }
                }
            }
            parentProperty = null;
            return null;
        }

        private static Expression CreatePropertyAccess(Expression parameter, List<PropertyInfo> parentProperty, PropertyInfo property)
        {
            Expression parentPropertyAccess = parameter;
            parentProperty.Reverse();
            foreach (var item in parentProperty)
            {

                parentPropertyAccess = Expression.MakeMemberAccess(parentPropertyAccess, item);

            }
            var propertyAccess = Expression.MakeMemberAccess(parentPropertyAccess, property);

            return propertyAccess;
        }
    }
}
