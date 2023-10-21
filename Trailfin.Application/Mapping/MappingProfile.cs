using AutoMapper;
using System.Reflection;

namespace Trailfin.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                if (instance.GetType().GetMethod("Mapping", BindingFlags.Instance | BindingFlags.NonPublic) != null)
                    throw new Exception($"mapping method is private on {instance.GetType().FullName}");
                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                var res = methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}