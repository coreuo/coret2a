using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public class LeaseMethodMember : MethodMember
    {
        private static readonly MethodMergeDelegate MethodMergeFactory = (o, n, rt, rtn, m) => new LeaseMethodMerge(o, n, rt, rtn, m.Cast<LeaseMethodMember>());

        public override MethodMergeDelegate Merger => MethodMergeFactory;

        public LeaseMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
        }

        public static bool Is(Object @object, INamedTypeSymbol @interface, IMethodSymbol original)
        {
            return original.Name.StartsWith("Lease") && @object.Dictionary[@interface].ContainsKey(original.ReturnType);
        }

        public class LeaseMethodMerge : MethodMerge<LeaseMethodMember>
        {
            public LeaseMethodMerge(Object @object, string name, string returnType, string returnTypeName, IEnumerable<LeaseMethodMember> members) : base(@object, name, returnType, returnTypeName, members)
            {
            }

            public override IEnumerable<Call> ResolveCalls()
            {
                yield return new Call(Object, this, Name, default, 0, $"Save.{ReturnTypeName}Store.Lease", string.Empty, true);
            }
        }
    }
}