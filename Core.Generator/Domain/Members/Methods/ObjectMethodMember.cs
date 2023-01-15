using System.Collections.Generic;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public class ObjectMethodMember : MethodMember
    {
        private static readonly MethodMergeDelegate MethodMergeFactory = (o, n, rt, rtn, p, m) => new ObjectMethodMerge(o, n, rt, rtn, p, m.Cast<ObjectMethodMember>());

        public override MethodMergeDelegate Merger => MethodMergeFactory;

        public double? Priority { get; }

        public ObjectMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
            Priority = original
                .GetAttributes()
                .SingleOrDefault(a => a.AttributeClass?.Name == "PriorityAttribute")
                ?.ConstructorArguments[0].Value as double?;
        }

        public string ResolveHandler()
        {
            var root = Interface.Name.Substring(1, Interface.Name.Length - 1);

            var types = new[] { Object.Name }.Concat(Interface.TypeParameters.Select(Dictionary.ResolveType));

            return $"{Interface.ContainingNamespace}.{root}Handlers<{string.Join(", ", types)}>";
        }

        public static bool Is(IMethodSymbol original)
        {
            var attributes = original.GetAttributes();

            var result = original.ReturnsVoid && original.MethodKind == MethodKind.Ordinary &&
                         (original.IsAbstract ||
                          !original.IsAbstract &&
                          original.Name.StartsWith("On") &&
                          attributes.Any(a => a.AttributeClass?.Name == "PriorityAttribute"));

            return result;
        }

        public class ObjectMethodMerge : MethodMerge<ObjectMethodMember>
        {
            public ObjectMethodMerge(Object @object, string name, string returnType, string returnTypeName, string parameters, IEnumerable<ObjectMethodMember> members) : base(@object, name, returnType, returnTypeName, parameters, members)
            {
            }

            public override IEnumerable<Call> ResolveCalls()
            {
                return Members
                    .Where(m => !m.Original.IsAbstract && m.Priority != null)
                    .Select(m => new Call(Object, this, m.Priority.Value, $"{m.ResolveHandler()}.{m.Name}", string.Join(", ", new[]{"this"}.Concat(Parameters.Select(p => $"{p.name}")))));
            }
        }
    }
}