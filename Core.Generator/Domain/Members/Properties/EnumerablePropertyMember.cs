using System.Collections.Generic;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public abstract class EnumerablePropertyMember : PropertyMember
    {
        public AttributeData SizeAttribute { get; }

        public INamedTypeSymbol Enumerable { get; }

        public ITypeSymbol Parameter { get; }

        protected EnumerablePropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
            SizeAttribute = original.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "SizeAttribute");

            Enumerable = (INamedTypeSymbol)((ITypeParameterSymbol)original.Type).ConstraintTypes[0];

            Parameter = Enumerable.TypeArguments[0];
        }

        public static bool Is(IPropertySymbol original, string @interface, string implementation)
        {
            return original.Type.Name.EndsWith(implementation) && original.Type is ITypeParameterSymbol type && type.ConstraintTypes.Length == 1 && type.ConstraintTypes[0] is INamedTypeSymbol enumerable && enumerable.Name == @interface && enumerable.TypeArguments.Length == 1;
        }

        public override string ToString()
        {
            return $"[{SizeAttribute}] {Original.Type} {Original.Name} of {Interface}";
        }

        public abstract class EnumerablePropertyMerge<T> : PropertyMerge<T>
            where T : EnumerablePropertyMember
        {
            public Object Item { get; }

            public int Size { get; }

            protected EnumerablePropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<T> members) : base(@object, name, type, typeName, members)
            {
                var parameter = Members
                    .Select(m => m.Dictionary.ResolveType(m.Parameter))
                    .Distinct()
                    .Single();

                Item = Object.Root.Objects[parameter];

                Size = Members
                    .Where(m => m.SizeAttribute != null)
                    .Where(m => m.SizeAttribute.ConstructorArguments.Length == 1)
                    .Select(m => m.SizeAttribute.ConstructorArguments[0].Value)
                    .OfType<int>()
                    .Distinct()
                    .SingleOrDefault();
            }
        }
    }
}
