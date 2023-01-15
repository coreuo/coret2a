using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public class ReleaseObjectMethodMember : MethodMember
    {
        private static readonly MethodMergeDelegate MethodMergeFactory = (o, n, rt, rtn, p, m) => new ReleaseObjectMethodMerge(o, n, rt, rtn, p, m.Cast<ReleaseObjectMethodMember>());

        public override MethodMergeDelegate Merger => MethodMergeFactory;

        public ReleaseObjectMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
        }

        public static bool Is(Object @object, INamedTypeSymbol @interface, IMethodSymbol original)
        {
            return original.Name.StartsWith("Release") &&
                   original.Parameters.Length == 1 &&
                   original.Parameters.Single() is IParameterSymbol releaseParameter &&
                   @object.Dictionary[@interface].ContainsKey(releaseParameter.Type);
        }

        public class ReleaseObjectMethodMerge : MethodMerge<ReleaseObjectMethodMember>
        {
            public ReleaseObjectMethodMerge(Object @object, string name, string returnType, string returnTypeName, string parameters, IEnumerable<ReleaseObjectMethodMember> members) : base(@object, name, returnType, returnTypeName, parameters, members)
            {
            }

            public override IEnumerable<Call> ResolveCalls()
            {
                yield return new Call(Object, this, 0, $"Pool.Save.{Parameters.Single().type}Store.Release", Parameters.Single().name);
            }
        }
    }
}