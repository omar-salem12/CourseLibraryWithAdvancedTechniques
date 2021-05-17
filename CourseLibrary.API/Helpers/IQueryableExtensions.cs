using CourseLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtensions
    {
        private static string orderByString;

        // private static string orderByString;

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string,PropertyMappingValue> mappingDictionary)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));

            }
            if(mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if(string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }


            //the orderBy string is separated by ","
            var orderByAfterSplit = orderBy.Split(',');


            //apply each orderby clause in revers order 
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                // trim the orderBy cluse, as it might contain leading
                // or trailing spacing. Can't trim the var in foreach
                var trimedOrderByClause = orderByClause.Trim();

                var orderDescending = trimedOrderByClause.EndsWith("desc");

                var indexOfFirstSpace = trimedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimedOrderByClause : trimedOrderByClause.Remove(indexOfFirstSpace);


                // find the matching property
                if(!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"Key mapping for {propertyName} is missing");
                }

                //get the propertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if(propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }


                //Run through the property names
                foreach (var destinationProperty in propertyMappingValue.DestionationProperties)

                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    orderByString = orderByString +
                        (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }


    }
}
