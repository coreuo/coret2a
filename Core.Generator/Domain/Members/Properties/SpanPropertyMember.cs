using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class SpanPropertyMember : PropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new SpanPropertyMerge(o, n, t, tn, m.Cast<SpanPropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public AttributeData SizeAttribute { get; }

        public ITypeSymbol GenericType { get; }

        public SpanPropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
            SizeAttribute = original.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "SizeAttribute");

            GenericType = ((INamedTypeSymbol)original.Type).TypeArguments[0];
        }

        public static bool Is(IPropertySymbol original)
        {
            return original.Type.Name == "Span";
        }

        public override string ToString()
        {
            return $"[{SizeAttribute}] {Original.Type} {Original.Name} of {Interface}";
        }

        public class SpanPropertyMerge : PropertyMerge<SpanPropertyMember>
        {
            public ISymbol GenericType { get; }

            public int Size { get; }

            public Property Property { get; private set; }

            public SpanPropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<SpanPropertyMember> members) : base(@object, name, type, typeName, members)
            {
                GenericType = Members.Select(m => m.GenericType).Distinct(SymbolEqualityComparer.Default).Single();

                Size = Members
                    .Where(m => m.SizeAttribute != null)
                    .Where(m => m.SizeAttribute.ConstructorArguments.Length == 1)
                    .Select(m => m.SizeAttribute.ConstructorArguments[0].Value)
                    .OfType<int>()
                    .Distinct()
                    .Single();
            }

            public override string ResolveGetter()
            {
                return $"get => this.GetSpan<{Object.Name}, {GenericType}>({Property.Index}, {Size});";
            }

            public override string ResolveSize()
            {
                return $"{Size} * sizeof({GenericType})";
            }

            public override IEnumerable<Property> ResolveProperties()
            {
                yield return Property = ResolveProperty();
            }
        }
    }
}
