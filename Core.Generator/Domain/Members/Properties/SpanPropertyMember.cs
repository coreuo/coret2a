using System.Collections.Generic;
using System.Collections.Immutable;
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

                var source = Members
                    .GroupBy(m => m.Original, SymbolEqualityComparer.Default)
                    .Select(g => g.First())
                    .ToImmutableArray();

                var sizedSource = source
                    .Where(m => m.SizeAttribute != null)
                    .Where(m => m.SizeAttribute.ConstructorArguments.Length == 1)
                    .ToImmutableArray();

                var sizes = sizedSource
                    .Select(m => m.SizeAttribute.ConstructorArguments[0].Value)
                    .OfType<int>()
                    .ToImmutableArray();

                if (!sizes.Any())
                {
                    foreach (var member in source)
                    {
                        Object.Root.Context.ReportDiagnostic(
                            Diagnostic.Create(
                                new DiagnosticDescriptor(
                                    "LG0002",
                                    "Span size attribute is missing",
                                    "Span size attribute is missing {0}",
                                    "Attributes",
                                    DiagnosticSeverity.Error,
                                    true),
                                null,
                                $"{member.Original}"));
                    }
                }
                else if (sizes.Length > 1)
                {
                    foreach (var member in sizedSource)
                    {
                        Object.Root.Context.ReportDiagnostic(
                            Diagnostic.Create(
                                new DiagnosticDescriptor(
                                    "LG0003",
                                    "Multiple span size attributes",
                                    "Multiple span size attributes detected {0}",
                                    "Attributes",
                                    DiagnosticSeverity.Error,
                                    true),
                                null,
                                $"{member.Original}"));
                    }
                }

                Size = sizes.Single();
            }

            public override string ResolveGetter()
            {
                return $"get => this.GetSpan<{Object.Name}, {GenericType}>({ResolveOffset(Property)}, {Size});";
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
