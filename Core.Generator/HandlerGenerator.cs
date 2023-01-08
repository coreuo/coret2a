using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator
{
    [Generator]
    public class HandlerGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var interfaces = context.Compilation.Assembly.GetEntityAndElementInterfaces();

            foreach (var (name, content) in interfaces.GroupBy(i => ($"{i.ContainingNamespace}", i.Name.Substring(1, i.Name.Length - 1))).Select(g => CreateHandlersFile(context.Compilation.Assembly, g.Key, g)))
            {
                context.AddSource(name, content);
            }
        }

        public static (string name, string content) CreateHandlersFile(IAssemblySymbol assembly, (string @namespace, string name) labels, IEnumerable<INamedTypeSymbol> interfaces)
        {
            var file = $@"using Core.Abstract;

namespace {labels.@namespace};
{string.Join(Environment.NewLine, interfaces.Select(CreateHandlersClass))}";

            return ($"{labels.@namespace.Replace(".", "")}{labels.name}Handlers.cs", file);
        }

        public static string CreateHandlersClass(INamedTypeSymbol @interface)
        {
            var root = @interface.Name.Substring(1, @interface.Name.Length - 1);

            var rootType = $"T{root}";

            var rootName = $"{char.ToLower(root[0])}{root.Substring(1, root.Length - 1)}";

            var types = new[] { rootType }.Concat(@interface.TypeParameters.Select(p => p.Name));

            var rootConstraint = $@"
    where {rootType} : {@interface}";

            var constraints = new[]{rootConstraint}.Concat(@interface.TypeParameters.SelectMany(p => p.ConstraintTypes.Select(c => $@"
    where {p} : {c}"))).ToImmutableList();

            var handlers = @interface.GetNestedMethods(false).Select(m => m.ResolveEntityMethodHandlerDefinition(rootType, rootName));

            var @class = $@"
public static class {root}Handlers<{string.Join(", ", types)}>{string.Join(string.Empty, constraints)}
{{{string.Join(Environment.NewLine, handlers)}
}}";

            return @class;
        }
    }
}
