using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class BooleanPropertyMember : PropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new BooleanPropertyMerge(o, n, t, tn, m.Cast<BooleanPropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public AttributeData FlagAttribute { get; }

        public BooleanPropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
            FlagAttribute = original.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == "FlagAttribute");
        }

        public static bool Is(IPropertySymbol original)
        {
            return original.Type.Name == "Boolean";
        }

        public class BooleanPropertyMerge : PropertyMerge<BooleanPropertyMember>
        {
            public (string name, int index) Flag { get; }

            public Property Property { get; private set; }

            public BooleanPropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<BooleanPropertyMember> members) : base(@object, name, type, typeName, members)
            {
                Flag = Members
                    .Where(m => m.FlagAttribute != null)
                    .Where(m => m.FlagAttribute.ConstructorArguments.Length == 2)
                    .Where(m => m.FlagAttribute.ConstructorArguments[0].Value is string flag && !string.IsNullOrEmpty(flag) && m.FlagAttribute.ConstructorArguments[1].Value is int)
                    .Select(m => ((string)m.FlagAttribute.ConstructorArguments[0].Value, m.FlagAttribute.ConstructorArguments[1].Value is int value ? value : 0))
                    .Distinct()
                    .SingleOrDefault();
            }

            public override string ResolveGetter()
            {
                return Property != null
                    ? $"get => this.Get{TypeName}({Property.Index});"
                    : $"get => this.GetFlag({GetFlagPropertyIndex()}, {Flag.index});";
            }

            public override string ResolveSetter()
            {
                return Property != null
                    ? $"set => this.Set{TypeName}({Property.Index}, value);"
                    : $"set => this.SetFlag({GetFlagPropertyIndex()}, {Flag.index}, value);";
            }

            private int GetFlagPropertyIndex()
            {
                return Object.Properties.Single(p => p.Identifier == Flag.name).Index;
            }

            public override IEnumerable<Property> ResolveProperties()
            {
                if (Flag == default) yield return Property = ResolveProperty();
            }
        }
    }
}
