using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Properties
{
    public class SpanPropertyMember : PropertyMember
    {
        private static readonly PropertyMergeDelegate PropertyMergeFactory = (o, n, t, tn, m) => new SpanPropertyMerge(o, n, t, tn, m.Cast<SpanPropertyMember>());

        public override PropertyMergeDelegate Merger => PropertyMergeFactory;

        public AttributeData LengthAttribute { get; }

        public ITypeSymbol GenericType { get; }

        public SpanPropertyMember(Object @object, INamedTypeSymbol @interface, IPropertySymbol original) : base(@object, @interface, original)
        {
            LengthAttribute = original.GetAttributes().SingleOrDefault(a => a.IsAttribute("LengthAttribute"));

            GenericType = ((INamedTypeSymbol)original.Type).TypeArguments[0];
        }

        public static bool Is(IPropertySymbol original)
        {
            return original.Type.Name == "Span";
        }

        public override string ToString()
        {
            return $"[{LengthAttribute}] {Original.Type} {Original.Name} of {Interface}";
        }

        public class SpanPropertyMerge : PropertyMerge<SpanPropertyMember>
        {
            public ISymbol GenericType { get; }

            public int Length { get; }

            public Property Property { get; private set; }

            public SpanPropertyMerge(Object @object, string name, string type, string typeName, IEnumerable<SpanPropertyMember> members) : base(@object, name, type, typeName, members)
            {
                GenericType = Members.Select(m => m.GenericType).Distinct(SymbolEqualityComparer.Default).Single();

                var source = Members
                    .GroupBy(m => m.Original, SymbolEqualityComparer.Default)
                    .Select(g => g.First())
                    .ToImmutableArray();

                var lengthSource = source
                    .Where(m => m.LengthAttribute != null)
                    .Where(m => m.LengthAttribute.ConstructorArguments.Length == 1)
                    .ToImmutableArray();

                var lengths = lengthSource
                    .Select(m => m.LengthAttribute.ConstructorArguments[0].Value)
                    .OfType<int>()
                    .ToImmutableArray();

                if (!lengths.Any())
                {
                    foreach (var member in source)
                    {
                        Object.Root.Context.ReportDiagnostic(
                            Diagnostic.Create(
                                new DiagnosticDescriptor(
                                    "LG0002",
                                    "Span length attribute is missing",
                                    "Span length attribute is missing {0}",
                                    "Attributes",
                                    DiagnosticSeverity.Error,
                                    true),
                                null,
                                $"{member.Original}"));
                    }
                }
                else if (lengths.Length > 1)
                {
                    foreach (var member in lengthSource)
                    {
                        Object.Root.Context.ReportDiagnostic(
                            Diagnostic.Create(
                                new DiagnosticDescriptor(
                                    "LG0003",
                                    "Multiple span length attributes",
                                    "Multiple span length attributes detected {0}",
                                    "Attributes",
                                    DiagnosticSeverity.Error,
                                    true),
                                null,
                                $"{member.Original}"));
                    }
                }

                Length = lengths.Single();
            }

            public override string ResolveGetter()
            {
                return $"get => this.GetSpan<{Object.Name}, {GenericType}>({ResolveOffset(Property)}, {Length});";
            }

            public override string ResolveSize()
            {
                return $"{Length} * sizeof({GenericType})";
            }

            public override IEnumerable<Property> ResolveProperties()
            {
                yield return Property = ResolveProperty();
            }
        }
    }
}
