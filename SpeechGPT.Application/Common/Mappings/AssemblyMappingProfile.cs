using AutoMapper;
using System.Reflection;

namespace SpeechGPT.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromAssembly(assembly);

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mappableTypes = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMappable<>)))
                .ToList();

            foreach (var mappableType in mappableTypes)
            {
                var instance = Activator.CreateInstance(mappableType);
                var methodInfo = mappableType.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
