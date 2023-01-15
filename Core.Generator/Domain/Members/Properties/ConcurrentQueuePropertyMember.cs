using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class ConcurrentQueuePropertyMember : EnumerablePropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new ConcurrentQueuePropertyMerge(o, n, t, tn, m.Cast<ConcurrentQueuePropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public ConcurrentQueuePropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
        }

        public static bool Is(IPropertySymbol original)
        {
            return Is(original, "IProducerConsumerCollection", "ConcurrentQueue");
        }

        public class ConcurrentQueuePropertyMerge : EnumerablePropertyMerge<ConcurrentQueuePropertyMember>
        {
            public Property NextProperty { get; private set; }

            public Property TopProperty { get; private set; }

            public Property BottomProperty { get; private set; }

            public ConcurrentQueuePropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<ConcurrentQueuePropertyMember> members) : base(@object, name, type, typeName, members)
            {
            }

            public override string ResolveGetter()
            {
                return $"get => this.GetConcurrentQueue({ResolveOffset(TopProperty)}, Pool.Save.{Item.Name}Store, {ResolveOffset(NextProperty)});";
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
                yield return NextProperty = ResolveProperty(Item, $"{Object.Name}.{Name}.1.Next");

                yield return TopProperty = ResolveProperty(Object, $"{Name}.1.Top");

                yield return BottomProperty = ResolveProperty(Object, $"{Name}.2.Bottom");
            }
        }
    }
}