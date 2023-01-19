using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public class GetMethodMember : MethodMember
    {
        private static readonly MethodMergeDelegate MethodMergeFactory = (o, n, rt, rtn, m) => new GetMethodMerge(o, n, rt, rtn, m.Cast<GetMethodMember>());

        public override MethodMergeDelegate Merger => MethodMergeFactory;

        public GetMethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original) : base(@object, @interface, original)
        {
        }

        public static bool Is(Object @object, INamedTypeSymbol @interface, IMethodSymbol original)
        {
            return original.Name.StartsWith("Get") &&
                   original.Parameters.Length == 1 &&
                   original.Parameters.Single() is IParameterSymbol idParameter &&
                   idParameter.Type.IsValueType &&
                   idParameter.Type.ToString() == "int" &&
                   @object.Dictionary[@interface].ContainsKey(original.ReturnType);
        }

        public class GetMethodMerge : MethodMerge<GetMethodMember>
        {
            public GetMethodMerge(Object @object, string name, string returnType, string returnTypeName, IEnumerable<GetMethodMember> members) : base(@object, name, returnType, returnTypeName, members)
            {
            }

            public override IEnumerable<Call> ResolveCalls()
            {
                yield return new Call(Object, this, Name, default, 0, $"Save.{ReturnTypeName}Store.Get", Parameters.Single().name, true);
            }
        }
    }
}