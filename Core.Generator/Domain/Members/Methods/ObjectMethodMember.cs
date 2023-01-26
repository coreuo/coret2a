using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
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

        public Case Case { get; }

        public ImmutableArray<(string method, string parameter, int value)> Also { get; }

        public ObjectMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
            var attributes = original.GetAttributes();

            Priority = attributes
                .SingleOrDefault(a => a.IsAttribute("PriorityAttribute"))
                ?.ConstructorArguments[0].Value as double?;

            Link = attributes
                .SingleOrDefault(a => a.IsAttribute("LinkAttribute"))
                ?.ConstructorArguments[0].Value as string;

            Case = attributes
                .Where(a => a.IsAttribute("CaseAttribute") && a.ConstructorArguments.Length == 4)
                .Select(a => new Case(
                    a.ConstructorArguments[0].Value as string,
                    a.ConstructorArguments[1].Value as string,
                    a.ConstructorArguments[2].Value as string,
                    a.ConstructorArguments[3].Value))
                .SingleOrDefault();

            Also = attributes
                .Where(a => a.IsAttribute("AlsoAttribute") && a.ConstructorArguments.Length == 3)
                .Select(a => a.ConstructorArguments[2].Value is int value
                    ? (a.ConstructorArguments[0].Value as string,
                        a.ConstructorArguments[1].Value as string,
                        value)
                    : default)
                .ToImmutableArray();
        }

        public string ResolveHandler()
        {
            var types = Interface.TypeParameters.Select(Dictionary.ResolveType);

            return $"{Interface.ContainingNamespace}.{Interface.Name}<{string.Join(", ", types)}>";
        }

        public IEnumerable<Call> ResolveCalls(IEnumerable<(string fullType, string type, string name)> parameters)
        {
            yield return new Call(Priority ?? 0.0, Name.Substring(2), $"{ResolveHandler()}.{Name}", new[] { (Object.Name, Object.Name, "this") }.Concat(parameters.Skip(1)));

            foreach (var call in ResolveAdditionalCalls()) yield return call;
        }

        public IEnumerable<Call> ResolveAdditionalCalls()
        {
            var additional = Also.Select(c => new
            {
                Also = c,
                Calls = Object.MethodMembers
                    .OfType<ObjectMethodMerge>()
                    .Where(m => m.Name == c.method)
                    .SelectMany(m => m.ResolveSelfCalls())
            });

            foreach (var item in additional)
            {
                foreach (var call in item.Calls)
                {
                    call.Also = (item.Also.parameter, item.Also.value);

                    yield return call;
                }
            }
        }

        public static bool Is(IMethodSymbol original)
        {
            var attributes = original.GetAttributes();

            var result = original.ReturnsVoid && original.MethodKind == MethodKind.Ordinary &&
                         (original.IsAbstract ||
                          !original.IsAbstract &&
                          original.Name.StartsWith("On") &&
                          attributes.Any(a => a.IsAttribute("PriorityAttribute")));

            return result;
        }

        public class ObjectMethodMerge : MethodMerge<ObjectMethodMember>
        {
            public ObjectMethodMerge(Object @object, string name, string returnType, string returnTypeName, IEnumerable<ObjectMethodMember> members) : base(@object, name, returnType, returnTypeName, members)
            {
            }

            public override IEnumerable<Call> ResolveSelfCalls()
            {
                var calls = Members
                    .Where(m => !m.Original.IsAbstract && m.Priority != null)
                    .SelectMany(m => m.ResolveCalls(ResolveParameters(m)));

                foreach (var call in calls)
                {
                    call.Caller = Name;

                    yield return call;
                }
            }

            public override IEnumerable<Call> ResolveOtherCalls()
            {
                var calls = Members
                    .Where(m => m.Case != default && m.Priority != null)
                    .SelectMany(m => ResolveCaseCall(m));

                foreach (var call in calls) yield return call;
            }

            private IEnumerable<Call> ResolveCaseCall(ObjectMethodMember member, Case parent = null)
            {
                foreach (var call in ResolveSelfCalls())
                {
                    call.Caller = member.Case.Method;

                    call.Case = member.Case;

                    yield return call;
                }

                var nested = Object
                    .MethodMembers
                    .OfType<ObjectMethodMerge>()
                    .SelectMany(mr => mr.Members
                        .Where(mb => mb.Case?.Method == member.GroupName)
                        .Select(mb => new { Merge = mr, Member = mb }));

                foreach (var item in nested)
                {
                    var calls = item.Merge.ResolveCaseCall(item.Member, member.Case);

                    foreach (var call in calls)
                    {
                        call.Caller = member.Case.Method;

                        call.Case = new Case(member.Case, call.Case);

                        yield return call;
                    }
                }
            }
        }
    }
}