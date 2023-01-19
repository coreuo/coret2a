using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public class ObjectMethodMember : MethodMember
    {
        private static readonly MethodMergeDelegate MethodMergeFactory = (o, n, rt, rtn, m) => new ObjectMethodMerge(o, n, rt, rtn, m.Cast<ObjectMethodMember>());

        public override MethodMergeDelegate Merger => MethodMergeFactory;

        public double? Priority { get; }

        public string Link { get; }

        public override string GroupName => Link ?? base.GroupName;

        public (string method, string subject, string property, int value) Case { get; }

        public ObjectMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
            Priority = original
                .GetAttributes()
                .SingleOrDefault(a => a.AttributeClass?.Name == "PriorityAttribute")
                ?.ConstructorArguments[0].Value as double?;

            Link = original
                .GetAttributes()
                .SingleOrDefault(a => a.AttributeClass?.Name == "LinkAttribute")
                ?.ConstructorArguments[0].Value as string;

            Case = original
                .GetAttributes()
                .Where(a => a.AttributeClass?.Name == "CaseAttribute" && a.ConstructorArguments.Length == 3)
                .Select(a =>
                    a.ConstructorArguments[0].Value is string method &&
                    a.ConstructorArguments[1].Value is string property && 
                    a.ConstructorArguments[2].Value is int value
                        ? (method, (string)null, property, value)
                        : default)
                .Concat(original
                    .GetAttributes()
                    .Where(a => a.AttributeClass?.Name == "CaseAttribute" && a.ConstructorArguments.Length == 4)
                    .Select(a =>
                        a.ConstructorArguments[0].Value is string method &&
                        a.ConstructorArguments[1].Value is string subject &&
                        a.ConstructorArguments[2].Value is string property &&
                        a.ConstructorArguments[3].Value is int value
                            ? (method, subject, property, value)
                            : default))
                .SingleOrDefault();
        }

        public string ResolveHandler()
        {
            var root = Interface.Name.Substring(1, Interface.Name.Length - 1);

            var types = new[] { Object.Name }.Concat(Interface.TypeParameters.Select(Dictionary.ResolveType));

            return $"{Interface.ContainingNamespace}.{root}Handlers<{string.Join(", ", types)}>";
        }

        public Call ResolveCall(ObjectMethodMerge merge, string caller, (string subject, string property, int value) @case, IEnumerable<(string fullType, string type, string name)> parameters)
        {
            return new Call(Object, merge, caller, @case, Priority ?? 0.0, $"{ResolveHandler()}.{Name}", string.Join(", ", new[] { "this" }.Concat(parameters.Select(p => $"{p.name}"))));
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
            public ObjectMethodMerge(Object @object, string name, string returnType, string returnTypeName, IEnumerable<ObjectMethodMember> members) : base(@object, name, returnType, returnTypeName, members)
            {
            }

            public override IEnumerable<Call> ResolveCalls()
            {
                var calls = Members
                    .Where(m => !m.Original.IsAbstract && m.Priority != null)
                    .Select(m => m.ResolveCall(this, Name, default, ResolveParameters(m)));

                foreach (var call in calls) yield return call;

                var cases = Members
                    .Where(m => m.Case != default && m.Priority != null)
                    .Select(m => (m, c: m.Case))
                    .SelectMany(p => ResolveCaseCall(p.c));

                foreach (var @case in cases) yield return @case;
            }

            private IEnumerable<Call> ResolveCaseCall((string method, string subject, string property, int value) @case)
            {
                var calls = Members
                    .Where(m => !m.Original.IsAbstract && m.Priority != null)
                    .Select(m => m.ResolveCall(this, @case.method, (@case.subject, @case.property, @case.value), ResolveParameters(m)));

                foreach (var call in calls) yield return call;
            }
        }
    }
}