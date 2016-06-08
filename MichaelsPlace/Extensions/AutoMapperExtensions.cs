using AutoMapper;

namespace MichaelsPlace.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = expression.TypeMap;
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                if (sourceType.GetProperty(property) != null)
                    expression.ForSourceMember(property, o => o.Ignore());
                if (destinationType.GetProperty(property) != null)
                    expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
    }
}