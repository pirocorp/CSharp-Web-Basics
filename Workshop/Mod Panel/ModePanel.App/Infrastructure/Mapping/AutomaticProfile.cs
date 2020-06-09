namespace ModePanel.App.Infrastructure.Mapping
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Maps automatically this.CreateMap&lt;TSource, TDestination&gt; () maps
    /// if TDestination implements IMapFrom interface
    /// </summary>
    public class AutomaticProfile : PostProfile
    {
        public AutomaticProfile()
        {
            var allTypes = Assembly
                .GetEntryAssembly()
                .GetTypes();

            var mappedTypes = allTypes
                .Where(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .Select(t => new
                {
                    Destination = t,
                    Source = t.GetInterfaces()
                        .Where(i => i.IsGenericType
                                    && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                        .SelectMany(i => i.GetGenericArguments())
                        .First()
                })
                .ToList();

            foreach (var type in mappedTypes)
            {
                this.CreateMap(type.Source, type.Destination);
            }
        }
    }
}
