using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Domain;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class PropertyExtensions
    {
        public static IEnumerable<IPropertySymbol> GetNestedProperties(this INamedTypeSymbol type)
        {
            foreach (var property in type.GetMembers().OfType<IPropertySymbol>())
            {
                yield return property;
            }

            foreach (var @interface in type.AllInterfaces)
            {
                foreach (var property in @interface.GetMembers().OfType<IPropertySymbol>())
                {
                    yield return property;
                }
            }
        }

        public static Func<string> ResolveEntityProperty(this IPropertySymbol property, bool isEntity, string entity, List<(string name, string size)> metaData, ImmutableDictionary<ISymbol, string> genericDictionary, EntitiesMetaData entitiesMetaData, PropertiesMetaData propertiesMetaData, HashSet<string> propertyNames, Dictionary<string, int> flagDictionary)
        {
            var type = genericDictionary.ResolveType(property.Type);

            if (propertyNames.Contains(property.Name)) return null;

            else if(property.Name == "Id") return null;

            propertyNames.Add(property.Name);

            var resolver = property.ResolveEntityPropertyAccessors(isEntity, entity, type, metaData, genericDictionary, entitiesMetaData, propertiesMetaData, flagDictionary);

            return () => $@"
    public {type} {property.Name}
    {{{resolver()}
    }}";
        }

        public static Func<string> ResolveEntityPropertyAccessors(this IPropertySymbol property, bool isEntity, string entity, string type, List<(string name, string size)> metaData, ImmutableDictionary<ISymbol, string> genericDictionary, EntitiesMetaData entitiesMetaData, PropertiesMetaData propertiesMetaData, Dictionary<string, int> flagDictionary)
        {
            var index = metaData.Count;

            if (property.Name == "Identity" && property.Type.ToString() == "string")
            {
                return () => $@"
        get => ""{entity}"";";
            }
            else if (property.Type.Name == "Span")
            {
                var genericType = ((INamedTypeSymbol)property.Type).TypeArguments[0];

                var size = property.GetAttributes().Single(a => a.AttributeClass?.Name == "SizeAttribute").ConstructorArguments[0].Value;

            metaData.Add(($"nameof({property.Name})", $"{size}*sizeof({genericType})"));

                return () => $@"
        get => this.GetSpan<{entity}, {genericType}>({index}, {size});";
            }
            else if (property.Type.Name == "Boolean" && property.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "FlagAttribute") is AttributeData flagAttribute && flagAttribute.ConstructorArguments[0].Value is string flag)
            {
                var flagIndex = 0;

                if (flagAttribute.ConstructorArguments[1].Value is int @explicit && @explicit >= 0) flagIndex = @explicit;
                else if (flagDictionary.ContainsKey(flag)) flagIndex = flagDictionary[flag]++;
                else flagDictionary[flag] = 1;

                return () =>
                {
                    index = metaData.FindIndex(s => s.name == $"nameof({flag})");

                    return $@"
        get => this.GetFlag({index}, {flagIndex});
        set => this.SetFlag({index}, {flagIndex}, value);";
                };
            }
            else if (property.Type.Name == "DateTime")
            {
                metaData.Add(($"nameof({property.Name})", "sizeof(long)"));

                return () => $@"
        get => new DateTime(this.GetInt64({index}));
        set => this.SetInt64({index}, value.Ticks);";
            }

            else if (property.Type.IsValueType)
            {
                metaData.Add(($"nameof({property.Name})", $"sizeof({type})"));
                
                return () => $@"
        get => this.Get{property.Type.Name}({index});
        set => this.Set{property.Type.Name}({index}, value);";
            }

            else if (type.StartsWith("Core.Launcher.Collections.ConcurrentQueue"))
            {
                var collectionEntityType = type.ResolveCollectionEntityType();

                var collectionEntityMetaData = propertiesMetaData.Get(collectionEntityType);

                var collectionEntityPropertyIndex = collectionEntityMetaData.Count;

                collectionEntityMetaData.Add(($"\"{entity}.{property.Name}.Next\"", "sizeof(int)"));

                metaData.Add(($"\"{property.Name}.Top\"", "sizeof(int)"));

                metaData.Add(($"\"{property.Name}.Bottom\"", "sizeof(int)"));

                return () => $@"
        get => this.GetConcurrentQueue({index}, Pool.Save.{collectionEntityType}Store, {collectionEntityPropertyIndex});";
            }

            else if (type.StartsWith("Core.Launcher.Collections.List"))
            {
                var collectionEntityType = type.ResolveCollectionEntityType();

                var collectionEntityMetaData = propertiesMetaData.Get(collectionEntityType);

                var collectionEntityPropertyIndex = collectionEntityMetaData.Count;

                collectionEntityMetaData.Add(($"\"{entity}.{property.Name}.Owner\"", "sizeof(int)"));

                collectionEntityMetaData.Add(($"\"{entity}.{property.Name}.Next\"", "sizeof(int)"));

                collectionEntityMetaData.Add(($"\"{entity}.{property.Name}.Previous\"", "sizeof(int)"));

                metaData.Add(($"\"{property.Name}.Count\"", "sizeof(int)"));

                metaData.Add(($"\"{property.Name}.Top\"", "sizeof(int)"));

                metaData.Add(($"\"{property.Name}.Bottom\"", "sizeof(int)"));

                return () => $@"
        get => this.GetList({index}, Pool.Save.{collectionEntityType}Store, {collectionEntityPropertyIndex});";
            }
            else if (type.StartsWith($"IEntity<{entity}>.Array"))
            {
                var arrayType = type.Substring(type.IndexOf(">", StringComparison.InvariantCulture) + 1);

                var collectionElementType = $"{arrayType.ResolveCollectionEntityType()}<{entity}>";

                var size = property.GetAttributes().Single(a => a.AttributeClass?.Name == "SizeAttribute").ConstructorArguments[0].Value;

                metaData.Add(($"\"{property.Name}\"", $"{collectionElementType}.Size"));

                return () => $@"
        get => this.GetArray<{entity}, {collectionElementType}>({index}, {size});";
            }
            else if (genericDictionary.TryGetValue(property.Type, out var name))
            {
                var link = property.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "LinkAttribute") is AttributeData attribute ? (string)attribute.ConstructorArguments[0].Value : null;

                entitiesMetaData.Add($"Launcher.Domain.{name}", name);

                if(link == null) metaData.Add(($"nameof({name})", "sizeof(int)"));

                return () =>
                {
                    if (link != null)
                    {
                        index = metaData.FindIndex(s => s.name == $"\"{link}\"");
                    }

                    return $@"
        get => this.GetValue(Pool.Save.{name}Store, {index});
        set => this.SetValue(Pool.Save.{name}Store, {index}, value);";
                };
            }
            else
            {
                entitiesMetaData.Add($"{property.Type}", property.Type.Name, true);

                metaData.Add(($"nameof({property.Name})", "sizeof(int)"));

                return () => $@"
        get => this.GetValue(Pool.Save.{property.Type.Name}Store, {index});
        set => this.SetValue(Pool.Save.{property.Type.Name}Store, {index}, value);";
            }
        }
        
        public static string ResolveCollectionEntityType(this string collection)
        {
            return collection.Substring(collection.IndexOf('<') + 1, collection.IndexOf('>') - collection.IndexOf('<') - 1);
        }
    }
}
