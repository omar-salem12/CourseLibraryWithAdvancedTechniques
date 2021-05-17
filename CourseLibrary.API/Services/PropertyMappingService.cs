using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {

        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() { "Id"}) },
                {"MainCategory", new PropertyMappingValue(new List<string>() { "MainCategory"}) },
                { "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth"}, true)},
                { "Name" , new PropertyMappingValue(new List<string>() { "firstName", "LastName"})}
            };

        private IList<IPropertyMappping> _propertyMappings = new List<IPropertyMappping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource,TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if(string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ","

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)

            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if(!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
            }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
               <TSource, TDestination>()
        {
            //get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact [roperty mapping instance " +
                $"for <{typeof(TSource)}, {typeof(TDestination)}");
        }
    }
}
