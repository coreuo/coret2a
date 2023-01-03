﻿using System.Collections.Generic;
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

        public static string ResolveEntityProperty(this IPropertySymbol property, string entity, List<string> metaData, ImmutableDictionary<ISymbol, string> genericDictionary, EntitiesMetaData entitiesMetaData, PropertiesMetaData propertiesMetaData, HashSet<string> propertyNames)
        {
            var type = genericDictionary.ResolveType(property.Type);

            if (propertyNames.Contains(property.Name)) return null;

            else if(property.Name == "Id") return null;

            propertyNames.Add(property.Name);

            return $@"
    public {type} {property.Name}
    {{{property.ResolveEntityPropertyAccessors(entity, type, metaData, genericDictionary, entitiesMetaData, propertiesMetaData)}
    }}";
        }

        public static string ResolveEntityPropertyAccessors(this IPropertySymbol property, string entity, string type, List<string> metaData, ImmutableDictionary<ISymbol, string> genericDictionary, EntitiesMetaData entitiesMetaData, PropertiesMetaData propertiesMetaData)
        {
            var index = metaData.Count;

            if (property.Name == "Identity" && property.Type.ToString() == "string")
            {
                return $@"
        get => ""{entity}"";";
            }
            else if (property.Type.Name == "Span")
            {
                var genericType = ((INamedTypeSymbol)property.Type).TypeArguments[0];

                var size = property.GetAttributes().Single(a => a.AttributeClass?.Name == "SizeAttribute").ConstructorArguments[0].Value;

            metaData.Add($@"
            new(nameof({property.Name}), {size}*sizeof({genericType}))");

                return $@"
        get => this.GetSpan<{entity}, {genericType}>({index}, {size});";
            }

            else if (property.Type.Name == "DateTime")
            {
                metaData.Add($@"
            new(nameof({property.Name}), sizeof(long))");

                return $@"
        get => new DateTime(this.GetInt64({index}));
        set => this.SetInt64({index}, value.Ticks);";
            }

            else if (property.Type.IsValueType)
            {
                metaData.Add($@"
            new(nameof({property.Name}), sizeof({type}))");
                
                return $@"
        get => this.Get{property.Type.Name}({index});
        set => this.Set{property.Type.Name}({index}, value);";
            }

            else if (type.StartsWith("Core.Launcher.Collections.ConcurrentQueue"))
            {
                var collectionEntityType = type.ResolveCollectionEntityType();

                var collectionEntityMetaData = propertiesMetaData.Get(collectionEntityType);

                var collectionEntityPropertyIndex = collectionEntityMetaData.Count;

                collectionEntityMetaData.Add($@"
            new(""{entity}.{property.Name}.Next"", sizeof(int))");

                metaData.Add($@"
            new(""{property.Name}.Top"", sizeof(int))");

                metaData.Add($@"
            new(""{property.Name}.Bottom"", sizeof(int))");

                return $@"
        get => this.GetConcurrentQueue({index}, Pool.Save.{collectionEntityType}Store, {collectionEntityPropertyIndex});";
            }

            else if (type.StartsWith("Core.Launcher.Collections.List"))
            {
                var collectionEntityType = type.ResolveCollectionEntityType();

                var collectionEntityMetaData = propertiesMetaData.Get(collectionEntityType);

                var collectionEntityPropertyIndex = collectionEntityMetaData.Count;

                collectionEntityMetaData.Add($@"
            new(""{entity}.{property.Name}.Owner"", sizeof(int))");

                collectionEntityMetaData.Add($@"
            new(""{entity}.{property.Name}.Next"", sizeof(int))");

                collectionEntityMetaData.Add($@"
            new(""{entity}.{property.Name}.Previous"", sizeof(int))");

                metaData.Add($@"
            new(""{property.Name}.Count"", sizeof(int))");

                metaData.Add($@"
            new(""{property.Name}.Top"", sizeof(int))");

                metaData.Add($@"
            new(""{property.Name}.Bottom"", sizeof(int))");

                return $@"
        get => this.GetList({index}, Pool.Save.{collectionEntityType}Store, {collectionEntityPropertyIndex});";
            }
            else if (genericDictionary.TryGetValue(property.Type, out var name))
            {
                entitiesMetaData.Add($"Launcher.Domain.{name}", name);

                metaData.Add($@"
            new(nameof({name}), sizeof(int))");

                return $@"
        get => this.GetValue(Pool.Save.{name}Store, {index});
        set => this.SetValue(Pool.Save.{name}Store, {index}, value);";
            }

            else
            {
                entitiesMetaData.Add($"{property.Type}", property.Type.Name, true);

                metaData.Add($@"
            new(nameof({property.Name}), sizeof(int))");

                return $@"
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