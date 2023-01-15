using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class ObjectPropertyMember : PropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new ObjectPropertyMerge(o, n, t, tn, m.Cast<ObjectPropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public AttributeData LinkAttribute { get; }

        public ObjectPropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
            LinkAttribute = original.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "LinkAttribute");
        }

        public static bool Is(IPropertySymbol original)
        {
            return original.Type.Kind == SymbolKind.TypeParameter;
        }

        public class ObjectPropertyMerge : PropertyMerge<ObjectPropertyMember>
        {
            public Property Property { get; private set; }

            public string Link { get; set; }

            public ObjectPropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<ObjectPropertyMember> members) : base(@object, name, type, typeName, members)
            {
                Link = Members
                    .Where(m => m.LinkAttribute != null)
                    .Where(m => m.LinkAttribute.ConstructorArguments.Length == 1)
                    .Select(m => m.LinkAttribute.ConstructorArguments[0].Value)
                    .OfType<string>()
                    .Distinct()
                    .SingleOrDefault();
            }

            public override string ResolveGetter()
            {
                var property = GetProperty();

                return $"get => this.GetValue(Pool.Save.{Type}Store, {property.Index});";
            }

            public override string ResolveSetter()
            {
                var property = GetProperty();

                return $"set => this.SetValue(Pool.Save.{Type}Store, {property.Index}, value);";
            }

            private Property GetProperty()
            {
                return Link == null ? Property : Object.Properties.Single(p => p.Identifier == Link);
            }

            public override string ResolveSize()
            {
                return "sizeof(int)";
            }

            public override IEnumerable<Property> ResolveProperties()
            {
                if (Link != null) yield break;

                yield return Property = ResolveProperty();
            }
        }
    }
}