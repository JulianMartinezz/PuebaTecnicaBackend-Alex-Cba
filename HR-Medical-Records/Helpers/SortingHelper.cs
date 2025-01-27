using HR_Medical_Records.DTOs.SortingDTOs;
using System.Linq.Expressions;
using System.Reflection;

namespace HR_Medical_Records.Helpers
{
    /// <summary>
    /// A helper class for applying sorting and pagination to an <see cref="IQueryable{T}"/> query.
    /// </summary>
    public static class SortingHelper
    {
        /// <summary>
        /// Applies sorting and optional pagination to the provided query based on the filter parameters.
        /// </summary>
        /// <typeparam name="T">The type of the entities in the query.</typeparam>
        /// <param name="query">The query to which sorting and pagination will be applied.</param>
        /// <param name="filter">The filter containing sorting and pagination options.</param>
        /// <param name="applyPagination">Whether pagination should be applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> with sorting and optional pagination applied.</returns>
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

        /// <summary>
        /// Finds a property of the given type based on the property name, supporting nested properties.
        /// </summary>
        /// <param name="type">The type to search the property in.</param>
        /// <param name="propertyName">The name of the property to find.</param>
        /// <param name="parentProperty">A list of parent properties leading to the target property.</param>
        /// <param name="flag">A flag used for recursion depth limit.</param>
        /// <returns>The <see cref="PropertyInfo"/> of the found property, or null if not found.</returns>
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

        /// <summary>
        /// Creates an expression that accesses a nested property based on a list of parent properties.
        /// </summary>
        /// <param name="parameter">The parameter expression representing the object to access the property from.</param>
        /// <param name="parentProperty">The list of parent properties leading to the target property.</param>
        /// <param name="property">The target property to be accessed.</param>
        /// <returns>An <see cref="Expression"/> that accesses the nested property.</returns>
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
