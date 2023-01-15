using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Domain.Members.Methods
{
    public abstract class MethodMember
    {
        public delegate MethodMerge MethodMergeDelegate(Object @object, string name, string returnType, string returnTypeName, string parameters, IEnumerable<MethodMember> members);

        public Object Object { get; }

        public INamedTypeSymbol Interface { get; }

        public IMethodSymbol Original { get; }

        public string Name { get; }

        public string GroupName => Original.IsAbstract ? Name : Name.Substring(2);

        public string ReturnType { get; }

        public string ReturnTypeName { get; }

        public string Parameters { get; }

        public ImmutableDictionary<ISymbol, string> Dictionary => Object.Dictionary[Interface];

        public abstract MethodMergeDelegate Merger { get; }

        protected MethodMember(Object @object, INamedTypeSymbol @interface, IMethodSymbol original)
        {
            Object = @object;

            Interface = @interface;

            Original = original;

            Name = original.Name;

            if (original.ReturnType.Kind == SymbolKind.TypeParameter)
            {
                ReturnType = ReturnTypeName = Dictionary[original.ReturnType];
            }
            else
            {
                ReturnType = $"{original.ReturnType}";

                ReturnTypeName = original.ReturnType.Name;
            }

            Parameters = string.Join(",", original.Parameters.Select(p => Dictionary.ResolveType(p.Type)));
        }

        public static MethodMember Create(Object @object, INamedTypeSymbol @interface, IMethodSymbol original)
        {
            if (GetMethodMember.Is(@object, @interface, original)) return new GetMethodMember(@object, @interface, original);

            if (LeaseMethodMember.Is(@object, @interface, original)) return new LeaseMethodMember(@object, @interface, original);

            if (ReleaseObjectMethodMember.Is(@object, @interface, original)) return new ReleaseObjectMethodMember(@object, @interface, original);

            if (HoldMethodMember.Is(@object, @interface, original)) return new HoldMethodMember(@object, @interface, original);

            if (ReleaseCachedMethodMember.Is(original)) return new ReleaseCachedMethodMember(@object, @interface, original);

            if (ObjectMethodMember.Is(original)) return new ObjectMethodMember(@object, @interface, original);

            return null;

            throw new InvalidOperationException($"Unknown {@interface} member {original.Name}");
        }

        public override string ToString()
        {
            return $"{ReturnType} {Name}({Parameters})";
        }

        public abstract class MethodMerge<T> : MethodMerge
            where T : MethodMember
        {
            public IImmutableSet<T> Members { get; }

            protected MethodMerge(Object @object, string name, string returnType, string returnTypeName, string parameters, IEnumerable<T> members) : base(@object, name, returnType, returnTypeName, parameters)
            {
                Members = members.ToImmutableHashSet();
            }
        }

        public abstract class MethodMerge
        {
            public Object Object { get; }

            public string Name { get; }

            public string ReturnType { get; }

            public string ReturnTypeName { get; }

            public ImmutableArray<(string fullType, string type, string name)> Parameters { get; }

            public IImmutableList<Call> Calls { get; set; }

            protected MethodMerge(Object o, string name, string returnType, string returnTypeName, string parameters)
            {
                Object = o;
                Name = name;
                ReturnType = returnType;
                ReturnTypeName = returnTypeName;
                Parameters = parameters
                    .Split(',')
                    .Where(p => p.Length > 0)
                    .Select(p => (p, p.Split('.').Last(), $"@{p.Split('.').Last().Substring(0, 1).ToLower()}{p.Split('.').Last().Substring(1)}"))
                    .ToImmutableArray();
            }

            public virtual string ResolveDeclaration()
            {
                return $"{ReturnType} {Name}({string.Join(", ", Parameters.Select(p => $"{p.fullType} {p.name}"))})";
            }

            public abstract IEnumerable<Call> ResolveCalls();

            public override string ToString()
            {
                return $"{ReturnType} {Name}({Parameters})";
            }
        }
    }
}
