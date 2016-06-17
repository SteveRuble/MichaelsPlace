using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using JetBrains.Annotations;

namespace MichaelsPlace.Extensions
{
    public static class DataTablesRequestExtensions
    {
        public static DataTablesResponse ApplyTo<TItem>(this IDataTablesRequest request, IQueryable<TItem> query)
        {
            var searched = ApplySearch(request, query);

            var filtered = ApplyFilters(request, searched);

            var sorted = ApplySort(request, filtered);

            var limited = sorted.Skip(request.Start).Take(request.Length);

            return DataTablesResponse.Create(request, query.Count(), ApplySearch(request, query).Count(), limited.ToList());

        }

        private static IQueryable<TItem> ApplySearch<TItem>(IDataTablesRequest request, IQueryable<TItem> query)
        {
            var search = request.Search;
            var parameter = Expression.Parameter(typeof(TItem));
            if (search != null && !string.IsNullOrEmpty(search.Value) && !search.IsRegex)
            {
                var searchableColumns = request.Columns.Where(c => c.IsSearchable)
                                               .Select(c => CreateSearchFilter<TItem>(parameter, c, search.Value))
                                               .Where(e => e != null)
                                               .ToList();
                if (searchableColumns.Any())
                {
                    var searchExpression = searchableColumns.Aggregate(Expression.OrElse);

                    var searched = query.Where(Expression.Lambda<Func<TItem, bool>>(searchExpression, parameter));
                    return searched;
                }
            }

            return query;
        }

        private static IQueryable<TItem> ApplyFilters<TItem>(IDataTablesRequest request, IQueryable<TItem> query)
        {
            var parameter = Expression.Parameter(typeof(TItem));
            var filteredColumns = request.Columns.Where(c => c.IsSearchable && c.Search != null && c.Search.Value.IsPresent()).ToList()
                                         .Select(c => CreateSearchFilter<TItem>(parameter, c))
                                         .Where(e => e != null)
                                         .ToList();
            if (filteredColumns.Any())
            {
                var filterExpression = filteredColumns.Aggregate(Expression.AndAlso);

                var filtered = query.Where(Expression.Lambda<Func<TItem, bool>>(filterExpression, parameter));
                return filtered;
            }

            return query;
        }

        private static IOrderedQueryable<TItem> ApplySort<TItem>(IDataTablesRequest request, IQueryable<TItem> filtered)
        {
            var itemType = typeof(TItem);
            return request.Columns.Where(c => c.IsSortable && c.Sort != null)
                          .OrderBy(c => c.Sort.Order)
                          .Aggregate(filtered.OrderBy(x => 1), (f, c) =>
                          {
                              var property = itemType.GetProperty(c.Field, BindingFlags.Public|BindingFlags.Instance|BindingFlags.IgnoreCase);
                              if (property == null)
                              {
                                  return f;
                              }

                              var method = typeof(DataTablesRequestExtensions).GetMethod("ApplySortingColumn", BindingFlags.Static | BindingFlags.NonPublic)
                                                                            .MakeGenericMethod(itemType, property.PropertyType);
                              return (IOrderedQueryable<TItem>) method.Invoke(null, new object[] {f, c});
                          });
        }

        private static Expression CreateSearchFilter<TItem>(ParameterExpression parameter, IColumn column, string searchString = null)
        {
            var itemType = typeof(TItem);
            var property = itemType.GetProperty(column.Field, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
            {
                return null;
            }

            searchString = searchString ?? column.Search.Value;

            var memberAccess = Expression.Property(parameter, property);
            if (memberAccess.Type == typeof(string))
            {
                return Expression.Call(memberAccess, "Contains", Type.EmptyTypes, Expression.Constant(searchString));
            }
            try
            {
                var value = TypeDescriptor.GetConverter(memberAccess.Type).ConvertFromString(searchString);
                return Expression.Equal(memberAccess, Expression.Constant(value));
            }
            catch(FormatException)
            {
                /* incompatible */
            }


            return null;

        }

        [UsedImplicitly]
        private static IOrderedQueryable<TItem> ApplySortingColumn<TItem, TKey>(IOrderedQueryable<TItem> query, Column column)
        {
            var itemType = typeof(TItem);
            var parameter = Expression.Parameter(itemType);
            var memberAccess = Expression.Property(parameter, itemType, column.Field);
            var lambda = Expression.Lambda<Func<TItem, TKey>>(memberAccess, parameter);

            return column.Sort.Direction == SortDirection.Ascending
                ? query.ThenBy(lambda)
                : query.ThenByDescending(lambda);
        }
    }
}
