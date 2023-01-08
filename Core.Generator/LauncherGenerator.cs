using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Domain;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator
{
    [Generator]
    public class LauncherGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.Compilation.Assembly.HasLauncherAttribute()) return;

            var entitiesMetaData = new EntitiesMetaData();

            var implementations = CreateImplementations(context.Compilation, entitiesMetaData).ToList();

            foreach (var (name, content) in implementations.Select(f => f()))
            {
                context.AddSource(name, content);
            }

            var save = CreateSave(entitiesMetaData);

            context.AddSource(save.name, save.content);
        }

        private static (string name, string content) CreateSave(EntitiesMetaData entitiesMetaData)
        {
            var @class = $@"
using Core.Launcher;
using Core.Launcher.Domain;
using Launcher.Domain;

namespace Launcher;

public class Save : Save<Save>, ISave<Save>
{{{string.Join(Environment.NewLine, entitiesMetaData.GetStoreProperties())}

    public Save(string path = """") : base(path)
    {{{string.Join(Environment.NewLine, entitiesMetaData.GetStoreAssignments())}
    }}

    public override void Flush()
    {{{string.Join(Environment.NewLine, entitiesMetaData.GetStoreFlushes())}
    }}

    public static Pool<Save, TEntity> CreatePool<TEntity>(Save save, Schema schema, string? label = null, bool isSynchronized = false) where TEntity : IEntity<Pool<Save, TEntity>, TEntity>
    {{
        return new Pool<Save, TEntity>(save, schema, label, isSynchronized);
    }}
}}
";

            return ("Save.cs", @class);
        }

        private static IEnumerable<Func<(string name, string content)>> CreateImplementations(Compilation compilation, EntitiesMetaData entitiesMetaData)
        {
            var assemblies = compilation.GetReferencedAssemblies();

            var interfaces = assemblies
                .SelectMany(EntityExtensions.GetEntityAndElementInterfaces)
                .ToList();

            var implementations = interfaces
                .GroupBy(g => g.GetSubject(), (k, l) => (k, l.ToImmutableList()))
                .Select(g => new
                {
                    Subject = g.k,
                    Categories = g.Item2
                        .Where(i => i.HasVariant())
                        .GroupBy(i => i.GetVariant(), (m, n) => new
                        {
                            Variant = m,
                            Interfaces = n
                        }),
                    General = g.Item2.Where(i => !i.HasVariant())
                })
                .ToList();

            var generalSubjects = implementations
                .Where(g => !g.Categories.Any())
                .Select(g => g.Subject)
                .ToImmutableHashSet();

            var propertiesMetaData = new PropertiesMetaData();

            foreach (var implementation in implementations)
            {
                var subject = implementation.Subject;

                if (generalSubjects.Contains(subject))
                    yield return CreateImplementation(subject, implementation.General.ResolveInterfaceDictionary(subject, generalSubjects), entitiesMetaData, propertiesMetaData);
                else
                    foreach (var category in implementation.Categories)
                        yield return CreateImplementation($"{category.Variant}{subject}", category.Interfaces.Concat(implementation.General).ResolveInterfaceDictionary($"{category.Variant}{subject}", generalSubjects, category.Variant), entitiesMetaData, propertiesMetaData);
            }
        }

        private static Func<(string name, string content)> CreateImplementation(string name, ImmutableDictionary<ISymbol, ImmutableDictionary<ISymbol, string>> interfaceDictionary, EntitiesMetaData entitiesMetaData, PropertiesMetaData propertiesMetaData)
        {
            var isEntity = interfaceDictionary.Keys.Any(i => i.GetAttributes().Any(a => a.IsEntityAttribute()));

            if (isEntity)
                entitiesMetaData.Add($"Launcher.Domain.{name}", name, false, interfaceDictionary.Keys.Any(i => i.GetAttributes().Any(a => a.IsSynchronizedAttribute())));

            var inheritance = interfaceDictionary.Select(i => ((INamedTypeSymbol)i.Key).ResolveTypeParameters(p => i.Value[p])).ToList();

            var metaData = propertiesMetaData.Get(name, isEntity);

            var propertyNames = new HashSet<string>();

            var flagDictionary = new Dictionary<string, int>();

            var properties = interfaceDictionary.SelectMany(i => ((INamedTypeSymbol)i.Key).GetNestedProperties().Select(p => p.ResolveEntityProperty(isEntity, name, metaData, i.Value, entitiesMetaData, propertiesMetaData, propertyNames, flagDictionary)).Where(l => l != null)).ToList();

            var calls = interfaceDictionary.SelectMany(i => ((INamedTypeSymbol)i.Key).GetNestedMethods(false).Select(m => (((INamedTypeSymbol)i.Key).ResolveHandlers(name, i.Value), new ResolvedMethod(m, i.Value, true), m))).GroupBy(i => i.Item2, g => g).ToImmutableDictionary(g => g.Key, g => g.Select(i => (i.Item1, i.m)).ToImmutableList());

            var methods = interfaceDictionary.SelectMany(i => ((INamedTypeSymbol)i.Key).GetNestedMethods(true).Where(m => m.MethodKind == MethodKind.Ordinary).Select(m => m.ResolveEntityMethodDefinition(i.Value, calls))).ToImmutableHashSet();

            if(isEntity)
                return () => ($"{name}.cs", $@"
using Core.Launcher;
using Core.Launcher.Domain;
using Core.Launcher.EntityExtensions;
using Core.Abstract.Domain;

namespace Launcher.Domain;

public struct {name} : IEntity<Pool<Save, {name}>, {name}>,
    {string.Join(@",
    ", inheritance)}
{{  
    public Pool<Save, {name}> Pool {{ get; }}

    public int Id {{ get; set; }}

    public int Free
    {{
        get => this.GetInt32(0);
        set => this.SetInt32(0, value);
    }}
    {string.Join(Environment.NewLine, properties.Select(p => p()))}

    public {name}(Pool<Save, {name}> pool, int id)
    {{
        Pool = pool;
        Id = id;
    }}

    public Pool GetPool() => Pool;

    public IStore<{name}> GetStore() => Pool;
    {string.Join(Environment.NewLine, methods)}

    public static Property[] GetProperties()
    {{
        return new Property[]
        {{{string.Join(",", metaData.Select(p => $@"
            new({p.name}, {p.size})"))}
        }};
    }}

    public const int Size = {string.Join(" + ", metaData.Select(p => p.size))};

    public static int GetPoolCapacity()
    {{
        return 10000;
    }}

    public static {name} Create(Pool<Save, {name}> pool, int id)
    {{
        return new {name}(pool, id);
    }}
}}");
            else
                return () => ($"{name}.cs", $@"
using Core.Launcher;
using Core.Launcher.Domain;
using Core.Launcher.ElementExtensions;
using Core.Abstract.Domain;

namespace Launcher.Domain;

public struct {name} : IElement<{name}>,
    {string.Join(@",
    ", inheritance)}
{{  
    public Core.Launcher.Collections.Array<{name}> Array {{ get; set; }}

    public int EntityId => Array.Id;

    public int EntityIndex => Array.Index;

    public int ElementId {{ get; set; }}

    public Property[] Properties => _properties;
    {string.Join(Environment.NewLine, properties.Select(p => p()))}

    public {name}(Core.Launcher.Collections.Array<{name}> array, int id)
    {{
        Array = array;
        ElementId = id;
    }}

    public Pool GetPool() => Array.Pool;
    {string.Join(Environment.NewLine, methods)}

    private static Property[] _properties = new Property[]
    {{{string.Join(",", metaData.Select(p => $@"
        new({p.name}, {p.size})"))}
    }};

    public const int Size = {string.Join(" + ", metaData.Select(p => p.size))};

    public static {name} Create(Core.Launcher.Collections.Array<{name}> array, int id)
    {{
        return new {name}(array, id);
    }}
}}");
        }
    }
}
