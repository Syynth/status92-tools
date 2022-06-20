using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

namespace Status92.Tools
{
    public static class TypeUtils
    {
        public static IEnumerable<Type> GetTypesForOpenGenericInterface(params Type[] interfaceTypes)
        {
            return interfaceTypes.SelectMany(_GetGenericInterfaceImpls);
        }

        private static IEnumerable<Type> _GetGenericInterfaceImpls(Type interfaceType)
        {
            var definedIn = interfaceType.Assembly.GetName().Name;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.GlobalAssemblyCache)
                .Where(assembly =>
                    assembly.GetName().Name == definedIn ||
                    assembly.GetReferencedAssemblies()
                        .Any(a => a.Name == definedIn))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass)
                .Where(t => t.ImplementsOpenGenericInterface(interfaceType));
        }

        public static IEnumerable<(TAttr, Type)> GetAttributeInstances<TAttr>() where TAttr : Attribute
        {
            string definedIn = typeof(TAttr).Assembly.GetName().Name;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.GlobalAssemblyCache)
                .Where(assembly =>
                    assembly.GetName().Name == definedIn ||
                    assembly.GetReferencedAssemblies()
                        .Any(a => a.Name == definedIn))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttributes(typeof(TAttr), true).Any())
                .SelectMany(type =>
                    type.GetCustomAttributes<TAttr>()
                        .Select(attr => (attr, type)));
        }

        public static IEnumerable<Type> GetClassesWithAttribute<TAttr>()
        {
            string definedIn = typeof(TAttr).Assembly.GetName().Name;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.GlobalAssemblyCache)
                .Where(assembly =>
                    assembly.GetName().Name == definedIn ||
                    assembly.GetReferencedAssemblies()
                        .Any(a => a.Name == definedIn))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttributes(typeof(TAttr), true).Any());
        }

        public delegate dynamic GenericAction<T>();

        public static dynamic InvokeDynamic(Type type, GenericAction<dynamic> action)
        {
            return InvokeDynamic(new[] { type }, action);
        }

        public static dynamic InvokeDynamic(IEnumerable<Type> types, GenericAction<dynamic> action)
        {
            return action
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(types.ToArray())
                .Invoke(null, new object[] { });
        }
    }
}