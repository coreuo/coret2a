using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class ListPropertyMember : EnumerablePropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new ListPropertyMerge(o, n, t, tn, m.Cast<ListPropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public ListPropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
        }

        public static bool Is(IPropertySymbol original)
        {
            return Is(original, "ICollection", "Collection");
        }

        public class ListPropertyMerge : EnumerablePropertyMerge<ListPropertyMember>
        {
            public Property OwnerProperty { get; private set; }

            public Property NextProperty { get; private set; }

            public Property PreviousProperty { get; private set; }

            public Property CountProperty { get; private set; }

            public Property TopProperty { get; private set; }

            public Property BottomProperty { get; private set; }

            public ListPropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<ListPropertyMember> members) : base(@object, name, type, typeName, members)
            {
            }

            public override string ResolveGetter()
            {
                return $"get => this.GetList({ResolveOffset(CountProperty)}, Pool.Save.{Item.Name}Store, {ResolveOffset(OwnerProperty)});";
            }

            public override string ResolveSize()
            {
                return "sizeof(int)";
            }

            public override string ResolveName(string name)
            {
                return $"\"{name}\"";
            }

            public override IEnumerable<Property> ResolveProperties()
            {
                yield return OwnerProperty = ResolveProperty(Item, $"{Object.Name}.{Name}.1.Owner");

                yield return NextProperty = ResolveProperty(Item, $"{Object.Name}.{Name}.2.Next");

                yield return PreviousProperty = ResolveProperty(Item, $"{Object.Name}.{Name}.3.Previous");

                yield return CountProperty = ResolveProperty(Object, $"{Name}.1.Count");

                yield return TopProperty = ResolveProperty(Object, $"{Name}.2.Top");

                yield return BottomProperty = ResolveProperty(Object, $"{Name}.3.Bottom");
            }
        }
    }
}